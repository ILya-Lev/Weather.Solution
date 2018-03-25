using Newtonsoft.Json;
using OpenWeatherMap.DataAccess.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenWeatherMap.DataAccess
{
	public class ImportLocationService
	{
		public IEnumerable<LocationDb> ImportLocations(string filePath)
		{
			var locations = ReadFile(filePath);

			var locationsDb = locations.Select(loc => new LocationDb
			{
				Id = loc.Id,
				Name = loc.Name,
				CountryCode = loc.CountryCode,
				Latitude = loc.Coordinates.Latitude,
				Longitude = loc.Coordinates.Longitude,
			})
			.ToList();

			return locationsDb;
		}

		private static IEnumerable<Location> ReadFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Cannot find file with supported locations data.", filePath);
			}

			//var locationFileLines = File.ReadLines(filePath);
			using (var stringReader = new StringReader(File.ReadAllText(filePath)))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<Location[]>(jsonReader);
			}
		}
	}
}
