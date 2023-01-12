using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs.Actions;
using Core.Models.Enums;
using Core.Services.GameProcess.Implementation;
using NUnit.Framework;

namespace Tests
{
	public class ResourceProcessor_Tests
	{
		[Test]
		public void ResourceProcessor_ReturnsEmptyList_WhenNoResourcesProvided()
		{
			var from = new Dictionary<ResourceType, int>();
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(0, toUse.Count);
		}

		[Test]
		public void ResourceProcessor_ReturnsEmptyList_WhenNoCostsProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 }
			};
			var costs = Array.Empty<ResourceCostData>();

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(0, toUse.Count);
		}

		[Test]
		public void ResourceProcessor_ReturnsTheSameTotalAmountAsRequired()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(2, toUse.Sum(r => r.Value));
		}

		[Test]
		public void ResourceProcessor_ReturnsTheSameTotalAmountAsRequired_WhenMoreResourcesProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 10 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(2, toUse.Sum(r => r.Value));
		}

		[Test]
		public void ResourceProcessor_ReturnsTheSameTotalAmountAsRequired_WhenMoreResourcesProvidedAndCostRelationAny()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 1 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(2, toUse.Sum(r => r.Value));
		}

		[Test]
		public void ResourceProcessor_ReturnsTheSameTotalAmountAsRequired_WhenMoreResourcesProvidedAndCostRelationAnyAndSame()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				},
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water | ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(4, toUse.Sum(r => r.Value));
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectTypes_WhenMoreResourceTypesProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 10 },
				{ ResourceType.Water, 10 },
				{ ResourceType.Electricity, 10 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Electricity
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.IsTrue(toUse.First().Key == ResourceType.Electricity);
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectTypes_WhenMoreResourceTypesRequested()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 10 },
				{ ResourceType.Water, 10 },
				{ ResourceType.Electricity, 10 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.IsTrue(toUse.First().Key is ResourceType.Electricity or ResourceType.Water);
		}

		[Test]
		public void ResourceProcessor_ReturnsMultipleCorrectTypes_WhenMultipleTypesWithAnyRelationProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 1 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.IsTrue(toUse.Keys.All(r => r is ResourceType.Electricity or ResourceType.Water));
		}

		[Test]
		public void ResourceProcessor_ReturnsMultipleCorrectTypes_WhenAnyAndSameRelationProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				},
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.IsTrue(toUse.Keys.All(r =>
				r is ResourceType.Electricity or ResourceType.Water or ResourceType.Ore));
		}

		[Test]
		public void ResourceProcessor_ReturnsMultipleCorrectTypes_WhenAnyAndSameRelationProvidedInDifferentOrder()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 2 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				},
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.IsTrue(toUse.Keys.All(r =>
				r is ResourceType.Electricity or ResourceType.Water or ResourceType.Ore));
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectAmountOfSame_WhenAnyAndSameRelationProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 3,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				},
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(3, toUse[ResourceType.Ore]);
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectAmountOfSame_WhenAnyAndSameRelationProvidedInDifferentOrder()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				},
				new()
				{
					Amount = 3,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(3, toUse[ResourceType.Ore]);
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectAmountOfAny_WhenAnyAndSameRelationProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 3,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				},
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(2,
				toUse.Where(r => r.Key is ResourceType.Electricity or ResourceType.Water)
					.Sum(r => r.Value));
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectAmountOfAny_WhenAnyAndSameRelationProvidedInDifferentOrder()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 3 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Electricity | ResourceType.Water
				},
				new()
				{
					Amount = 3,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(2,
				toUse.Where(r => r.Key is ResourceType.Electricity or ResourceType.Water)
					.Sum(r => r.Value));
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectAmount_WhenMoreOfSameAndAnyAndSameRelationProvided()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 4 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 4,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				},
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Ore | ResourceType.Electricity | ResourceType.Water
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(4, toUse[ResourceType.Ore]);
		}

		[Test]
		public void ResourceProcessor_ReturnsCorrectAmount_WhenMoreOfSameAndAnyAndSameRelationProvidedInDifferentOrder()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 4 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 2,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Ore | ResourceType.Electricity | ResourceType.Water
				},
				new()
				{
					Amount = 4,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(4, toUse[ResourceType.Ore]);
		}

		[Test]
		public void ResourceProcessor_ReturnsAllResourcesOfCorrectTypes_When0AmountAndRelationAnyRequested()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Ore, 4 },
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 0,
					Relation = ResourceRelation.Any,
					Type = ResourceType.Ore | ResourceType.Electricity
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(5, toUse.Sum(r => r.Value));
			Assert.IsTrue(toUse.Keys.All(r => r is ResourceType.Ore or ResourceType.Electricity));
		}

		[Test]
		public void ResourceProcessor_ReturnsResourceOfCorrectTypeAndMaxAmount_When0AmountAndRelationSameRequested()
		{
			var from = new Dictionary<ResourceType, int>
			{
				{ ResourceType.Water, 1 },
				{ ResourceType.Electricity, 1 },
				{ ResourceType.Ore, 4 }
			};
			var costs = new ResourceCostData[]
			{
				new()
				{
					Amount = 0,
					Relation = ResourceRelation.Same,
					Type = ResourceType.Ore | ResourceType.Electricity
				}
			};

			var resourceProcessor = new ResourceProcessor();
			var (toUse, _) = resourceProcessor.Process(from, costs);

			Assert.AreEqual(4, toUse.Sum(r => r.Value));
			Assert.IsTrue(toUse.Keys.All(r => r is ResourceType.Ore));
		}
	}
}