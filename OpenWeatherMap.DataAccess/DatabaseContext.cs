using OpenWeatherMap.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OpenWeatherMap.DataAccess
{
	public class DatabaseContext : DbContext, IDatabaseContext
	{
		public DatabaseContext() : base("OpenWeatherMapDb")
		{
			Database.Log += Console.WriteLine;
		}

		public DbSet<LocationDb> Locations { get; set; }

		IQueryable<LocationDb> IDatabaseContext.Locations => Locations;

		public void AddRange(IEnumerable<LocationDb> items)
		{
			Locations.AddRange(items);
		}

		public void AddEntity(LocationDb item)
		{
			Locations.Add(item);
		}
	}
}
