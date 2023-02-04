using System.Collections.Generic;
using Core.Configs.Actions;
using Core.Models.Actions;
using Core.Models.Actions.Implementation;
using Core.Models.Enums;
using Core.Models.GameProcess.Implementation;
using Core.Utils;
using NUnit.Framework;

namespace Tests
{
	public class BaseAction_Tests
	{
		#region Mock classes

		private class MockActionConfig : IActionConfig
		{
			public ActionType Type { get; set; }
			public string DisplayName { get; set; }
			public ActionRepeatability Repeatability { get; set; }
			public byte Oxygen { get; set; }
			public ResourceCostData[] Resources { get; set; }
		}

		#endregion

		class BaseActionImpl : BaseAction
		{
			public BaseActionImpl(IActionConfig config) : base(config)
			{
			}

			protected override void ApplyAction()
			{
			}
		}

		[Test]
		public void BaseAction_ReturnsPerformerNotSet()
		{
			IAction action = new BaseActionImpl(new MockActionConfig { Oxygen = 5 });
			action.Performer = null;

			Assert.AreEqual(ActionResult.PerformerNotSet, action.Perform(false));
		}

		[Test]
		public void BaseAction_ReturnsNotSelected()
		{
			IAction action = new BaseActionImpl(new MockActionConfig { Oxygen = 5 });
			action.Selected.Value = false;
			action.Performer = new Player(Faction.Red);

			Assert.AreEqual(ActionResult.NotSelected, action.Perform(false));
		}

		[Test]
		public void BaseAction_ReturnsNotEnoughOxygen()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 2;
			IAction action = new BaseActionImpl(new MockActionConfig { Oxygen = 5 });
			action.Performer = performer;
			action.Selected.Value = true;

			Assert.AreEqual(ActionResult.NotEnoughOxygen, action.Perform(false));
		}

		[Test]
		public void BaseAction_ReturnsNotEnoughResources()
		{
			var config = new MockActionConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 5,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore | ResourceType.Plants
					}
				}
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = new Player(Faction.Red);
			action.Selected.Value = true;

			Assert.AreEqual(ActionResult.NotEnoughResources, action.Perform(false));
		}

		[Test]
		public void BaseAction_ReturnsNotEnoughResources_WhenResourcesDontComplyWithConfig()
		{
			var performer = new Player(Faction.Red);
			var config = new MockActionConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;
			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);

			Assert.AreEqual(ActionResult.NotEnoughResources, action.Perform(false));
		}

		[Test]
		public void BaseAction_ReturnsNoResourcesProvided_WhenNoResourcesProvided()
		{
			var performer = new Player(Faction.Red);
			var config = new MockActionConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 4,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;
			performer.AddResources(new ResourcePackage(ResourceType.Ore, 10));

			Assert.AreEqual(ActionResult.NoResourcesProvided, action.Perform(false));
		}

		[Test]
		public void BaseAction_ReturnsNoResourcesProvided_WhenResourcesDontComplyWithConfig()
		{
			var performer = new Player(Faction.Red);
			var config = new MockActionConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;
			performer.AddResources(new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 5 },
				{ ResourceType.Electricity, 5 },
			}));
			action.Resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Electricity, 5 },
			});

			Assert.AreEqual(ActionResult.NoResourcesProvided, action.Perform(false));
		}

		[Test]
		public void BaseAction_Performes()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockActionConfig
			{
				Repeatability = ActionRepeatability.None,
				Oxygen = 1,
				Resources = new ResourceCostData[]
					{ }
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;

			Assert.AreEqual(ActionResult.Success, action.Perform(false));
		}

		[Test]
		public void BaseAction_UsesOxygen_WhenRepeatAndActionNotRepeatable()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockActionConfig
			{
				Repeatability = ActionRepeatability.None,
				Oxygen = 1,
				Resources = new ResourceCostData[]
					{ }
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;

			Assert.AreEqual(ActionResult.Success, action.Perform(true));
			Assert.AreEqual(4, performer.Oxygen.Value);
		}

		[Test]
		public void BaseAction_DoesNotUseOxygen_WhenRepeatAndActionRepeatable()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockActionConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
					{ }
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;

			Assert.AreEqual(ActionResult.Success, action.Perform(true));
			Assert.AreEqual(5, performer.Oxygen.Value);
		}

		[Test]
		public void Selected_ResetsActionState_WhenSetToFalse()
		{
			var performer = new Player(Faction.Red);
			performer.Oxygen.Value = 5;
			var config = new MockActionConfig
			{
				Repeatability = ActionRepeatability.Repeatable,
				Oxygen = 1,
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};
			IAction action = new BaseActionImpl(config);
			action.Performer = performer;
			action.Selected.Value = true;

			var resources = new ResourcePackage(new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Electricity, 5 },
			});
			performer.AddResources(resources);

			action.Selected.Value = false;

			Assert.AreNotEqual(ActionResult.Success, action.CheckProvidedResources());
		}

		[Test]
		public void ResourcesRequired_ReturnsTrue_WhenResourcesInConfigPresent()
		{
			var config = new MockActionConfig
			{
				Resources = new ResourceCostData[]
				{
					new()
					{
						Amount = 3,
						Relation = ResourceRelation.Same,
						Type = ResourceType.Ore
					}
				}
			};

			IAction action = new BaseActionImpl(config);

			Assert.IsTrue(action.ResourcesRequired);
		}

		[Test]
		public void ResourcesRequired_ReturnsFalse_WhenNoResourcesInConfig()
		{
			var config = new MockActionConfig
			{
				Resources = new ResourceCostData[]
				{ }
			};

			IAction action = new BaseActionImpl(config);

			Assert.IsFalse(action.ResourcesRequired);
		}
	}
}