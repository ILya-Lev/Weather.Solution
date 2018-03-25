using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OpenWeatherMap.DataAccess.Model
{
	public class Location
	{
		[Required]
		public int Id { get; set; }

		public string Name { get; set; }

		[JsonProperty("country")]
		[StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be of 2 symbols")]
		public string CountryCode { get; set; }

		[JsonProperty("coord")]
		public Coordinates Coordinates { get; set; }
	}
}
