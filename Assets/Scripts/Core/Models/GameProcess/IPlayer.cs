using System.Collections.Generic;
using Core.Models.Actors;

namespace Core.Models.GameProcess
{
	public interface IPlayer : IResourceHolder, ITurnPerformer, ITechnologyUser
	{
		IEnumerable<IActor> Actors { get; }
	}
}