using System.Collections.Generic;
using System.Linq;
using Core.Configs.Actions;
using Core.Models.Enums;
using Core.Utils;
using UnityEngine;

namespace Core.Services.GameProcess.Implementation
{
	public class ResourceProcessor
	{
		private delegate bool HandleDelegate(
			ref Dictionary<ResourceType, int> toUse,
			ref Dictionary<ResourceType, int> left,
			ResourceCostData cost);

		public (Dictionary<ResourceType, int> toUse, Dictionary<ResourceType, int> left) Process(
			Dictionary<ResourceType, int> from, ResourceCostData[] costs)
		{
			var left = from.ToDictionary(p => p.Key, p => p.Value);
			var toUse = new Dictionary<ResourceType, int>();

			if (left.Sum(r => r.Value) < costs.Sum(c => c.Amount))
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
			ref Dictionary<ResourceType, int> toUse,
			ref Dictionary<ResourceType, int> left,
			ResourceCostData cost)
		{
			var anyAmount = cost.Amount == 0;
			var candidate = left
				.Where(r =>
					(r.Value >= cost.Amount || anyAmount)
					&& r.Key.Contains(cost.Type))
				.OrderByDescending(r => r.Value)
				.FirstOrDefault();
			if (candidate.Value <= 0) return false;
			var amountToTake = anyAmount
				? candidate.Value
				: candidate.Value - (candidate.Value - cost.Amount);
			if (toUse.ContainsKey(candidate.Key))
				toUse[candidate.Key] += amountToTake;
			else toUse.Add(candidate.Key, amountToTake);
			left[candidate.Key] -= amountToTake;
			return true;
		}

		private bool HandleAny(
			ref Dictionary<ResourceType, int> toUse,
			ref Dictionary<ResourceType, int> left,
			ResourceCostData cost)
		{
			var candidates = left
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
				if (toUse.ContainsKey(key))
					toUse[key] += amountToTake;
				else toUse.Add(key, amountToTake);
				left[key] -= amountToTake;
				candidates[key] -= amountToTake;
				if (count >= cost.Amount && !anyAmount) break;
			}

			return count >= cost.Amount;
		}
	}
}