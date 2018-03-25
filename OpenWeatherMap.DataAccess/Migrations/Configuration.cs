using System;
using System.Data.Entity.Validation;
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
				//var filePath = Path.Combine(Directory.GetCurrentDirectory(),
				//							@"PredefinedData\city.list.json.gz");
				var locations = importService.ImportLocations(@"D:\Projects\LIS\studying\pet_projects\weather\simple_client\Weather.Solution\OpenWeatherMap.DataAccess\PredefinedData\city.list.json.gz");
				context.Locations.AddRange(locations);

				try
				{
					context.SaveChanges();
				}
				catch (DbEntityValidationException exc)
				{
					foreach (var result in exc.EntityValidationErrors)
					{
						Console.WriteLine($"{result.Entry} has validation errors {string.Join("\n", result.ValidationErrors.Select(err => $"{err.PropertyName}: {err.ErrorMessage}"))}");
					}
				}
			}
		}
	}
}
