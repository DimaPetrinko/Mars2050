using Core.Configs.Actions.Enums;
using UnityEngine;

namespace Core.Configs.Actions.Implementation
{
	[CreateAssetMenu(fileName = nameof(DiscoverConfig), menuName = "Configs/Actions/" + nameof(DiscoverConfig))]
	public class DiscoverConfig : ActionConfig, IDiscoverConfig
	{
		public override ActionType Type => ActionType.Discover;
	}
}