using Configs.Actions.Interfaces;
using UnityEngine;

namespace Configs.Actions
{
	[CreateAssetMenu(fileName = nameof(DiscoverConfig), menuName = "Configs/Actions/" + nameof(DiscoverConfig))]
	public class DiscoverConfig : ActionConfig, IDiscoverConfig
	{
		public override ActionType Type => ActionType.Discover;
	}
}