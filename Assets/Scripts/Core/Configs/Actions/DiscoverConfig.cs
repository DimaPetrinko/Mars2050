using Core.Configs.Actions.Enums;
using Core.Configs.Actions.Interfaces;
using UnityEngine;

namespace Core.Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(DiscoverConfig), menuName = "Configs/Actions/" + nameof(DiscoverConfig))]
	public class DiscoverConfig : ActionConfig, IDiscoverConfig
	{
		public override ActionType Type => ActionType.Discover;
	}
}