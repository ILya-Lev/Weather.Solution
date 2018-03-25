using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace OpenWeatherMap.DataAccess.Model
{
	[DebuggerDisplay("{CountryCode}; {Name}")]
	public class LocationDb
	{
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		[StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be of 2 symbols")]
		public virtual string CountryCode { get; set; }

		public virtual double Longitude { get; set; }

		public virtual double Latitude { get; set; }
	}
}