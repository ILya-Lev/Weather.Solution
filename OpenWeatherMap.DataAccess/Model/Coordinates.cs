using Newtonsoft.Json;

namespace OpenWeatherMap.DataAccess.Model
{
	public class Coordinates
	{
		[JsonProperty("lon")]
		public double Longitude { get; set; }

		[JsonProperty("lat")]
		public double Latitude { get; set; }
	}
}