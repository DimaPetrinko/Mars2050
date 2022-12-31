using System.Collections.Generic;
using Core.Models.Technology;

namespace Core.Models.GameProcess
{
	public interface ITechnologyUser
	{
		IEnumerable<ITechnology> Technologies { get; }
	}
}