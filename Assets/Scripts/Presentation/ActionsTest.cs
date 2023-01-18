using System.Collections.Generic;
using System.Linq;
using Core.Configs.Actions.Implementation;
using Core.Models.Enums;
using Core.Services.GameProcess.Implementation;
using Core.Utils;
using UnityEngine;

namespace Presentation
{
	internal class ActionsTest : MonoBehaviour
	{
		[SerializeField] private ActionConfigs m_ActionConfigs;

		private void Awake()
		{
			var from = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Water, 15 },
				{ ResourceType.Ore, 3 },
				{ ResourceType.Plants, 4 },
				{ ResourceType.Electricity, 2 }
			});

			var cost = new[]
			{
				new ResourceCostData
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Electricity
				},
				new ResourceCostData
				{
					Amount = 10,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Ore | ResourceType.Water | ResourceType.Electricity | ResourceType.Plants
				},
				new ResourceCostData
				{
					Amount = 4,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore | ResourceType.Plants
				}
			};

			var resourceProcessor = new ResourceProcessor();

			var (toUse, left) = resourceProcessor.Process(from, cost);
			var leftLog = left.Content
				.Select(r => $"{r.Key} {r.Value}")
				.Aggregate((a, b) => $"{a}\n{b}");
			leftLog = leftLog.Insert(0, "Left:\n");
			Debug.Log(leftLog);

			var correctResourcesLog = toUse.Content
				.Select(r => $"{r.Key} {r.Value}")
				.Aggregate((a, b) => $"{a}\n{b}");
			correctResourcesLog = correctResourcesLog.Insert(0, "Correct:\n");
			Debug.Log(correctResourcesLog);
		}
	}
}