using OpenWeatherMap.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace OpenWeatherMap.DataAccess
{
	public interface IDatabaseContext
	{
		IQueryable<LocationDb> Locations { get; }
		void AddEntity(LocationDb item);
		void AddRange(IEnumerable<LocationDb> item);
		int SaveChanges();
	}
}