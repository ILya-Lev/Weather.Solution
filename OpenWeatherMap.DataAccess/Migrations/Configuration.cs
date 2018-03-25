using System.Linq;

namespace OpenWeatherMap.DataAccess.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<OpenWeatherMap.DataAccess.DatabaseContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(OpenWeatherMap.DataAccess.DatabaseContext context)
		{
			if (!context.Locations.Any())
			{
				var importService = new ImportLocationService();
				var locations = importService.ImportLocations("");
				context.Locations.AddRange(locations);
				context.SaveChanges();
			}
		}
	}
}
