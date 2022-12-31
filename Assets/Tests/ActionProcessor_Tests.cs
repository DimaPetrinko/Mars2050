using System.Collections.Generic;
using Core.Models.Actors;
using Core.Models.Enums;
using Core.Models.GameProcess;
using Core.Models.Technology;
using NUnit.Framework;

namespace Tests
{
	public class ActionProcessor_Tests
	{
		private class MockPlayer : IPlayer
		{
			public IEnumerable<ResourceType> Resources { get; }
			public int ResourcesCount { get; }
			public bool HasResource(ResourceType resource, int amount)
			{
				throw new System.NotImplementedException();
			}

			public void AddResources(ResourceType[] resources)
			{
				throw new System.NotImplementedException();
			}

			public void UseResources(ResourceType[] resources)
			{
				throw new System.NotImplementedException();
			}

			public Faction Faction { get; }
			public int Oxygen { get; set; }
			public void UseOxygen(int oxygen)
			{
				throw new System.NotImplementedException();
			}

			public int Roll()
			{
				throw new System.NotImplementedException();
			}

			public void EndTurn()
			{
				throw new System.NotImplementedException();
			}

			public IEnumerable<ITechnology> Technologies { get; }
			public IEnumerable<IActor> Actors { get; }
		}
		[Test]
		public void MoveAction_Fails()
		{
			// IGameConfig gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/Configs/GameConfig.asset");
			// IActionProcessor actionProcessor = new ActionProcessor();
			//
			// var mockPlayer = new MockPlayer();
			// mockPlayer.Oxygen = 10;
			// var result = actionProcessor.Move(mockPlayer, new Cell(), new Cell(),
			//     gameConfig.Actions.GetConfig<IMoveConfig>(ActionType.Move));
			//
			// Debug.Log(result);
			//
			// Assert.AreNotEqual(ActionResult.Success, result);
		}
	}
}