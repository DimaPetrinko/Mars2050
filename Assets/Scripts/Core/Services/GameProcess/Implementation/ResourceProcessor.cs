using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using UnityEngine;

namespace Core.Services.GameProcess.Implementation
{
	public class ResourceProcessor
	{
		private delegate bool HandleDelegate(
			ref ResourcePackage toUse,
			ref ResourcePackage left,
			ResourceCostData cost);

		public (ResourcePackage toUse, ResourcePackage left) Process(
			ResourcePackage from, ResourceCostData[] costs)
		{
			var left = ResourcePackage.Copy(from);
			var toUse = ResourcePackage.Empty();

			if (left.Content.Sum(r => r.Value) < costs.Sum(c => c.Amount))
				return (toUse, left);

			var handleDelegates = new Dictionary<ResourceRelation, HandleDelegate>
			{
				{ ResourceRelation.Same, HandleSame },
				{ ResourceRelation.Any, HandleAny }
			};

			costs = costs.OrderBy(c => c.Relation).ToArray();
			if (!costs.All(c => handleDelegates[c.Relation](ref toUse, ref left, c)))
				toUse.Clear();

			return (toUse, left);
		}

		private bool HandleSame(
			ref ResourcePackage toUse,
			ref ResourcePackage left,
			ResourceCostData cost)
		{
			var anyAmount = cost.Amount == 0;
			var candidate = left.Content
				.Where(r =>
					(r.Value >= cost.Amount || anyAmount)
					&& r.Key.Contains(cost.Type))
				.OrderByDescending(r => r.Value)
				.FirstOrDefault();
			if (candidate.Value <= 0) return false;
			var amountToTake = anyAmount
				? candidate.Value
				: candidate.Value - (candidate.Value - cost.Amount);
			if (toUse.Content.ContainsKey(candidate.Key))
				toUse.Content[candidate.Key] += amountToTake;
			else toUse.Content.Add(candidate.Key, amountToTake);
			left.Content[candidate.Key] -= amountToTake;
			return true;
		}

		private bool HandleAny(
			ref ResourcePackage toUse,
			ref ResourcePackage left,
			ResourceCostData cost)
		{
			var candidates = left.Content
				.Where(r => r.Value > 0 && r.Key.Contains(cost.Type))
				.ToDictionary(p => p.Key, p => p.Value);
			var count = 0;
			var keys = candidates.Keys.ToArray();
			var anyAmount = cost.Amount == 0;
			foreach (var key in keys)
			{
				var amount = candidates[key];
				count += amount;
				var amountToTake = anyAmount
					? amount
					: amount - Mathf.Max(count - cost.Amount, 0);
				if (toUse.Content.ContainsKey(key))
					toUse.Content[key] += amountToTake;
				else toUse.Content.Add(key, amountToTake);
				left.Content[key] -= amountToTake;
				candidates[key] -= amountToTake;
				if (count >= cost.Amount && !anyAmount) break;
			}

			return count >= cost.Amount;
		}
	}
}