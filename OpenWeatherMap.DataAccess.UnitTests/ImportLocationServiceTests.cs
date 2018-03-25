using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace OpenWeatherMap.DataAccess.UnitTests
{
	public class ImportLocationServiceTests
	{
		[Fact]
		public void ImportLocations_AbsentFile_ThrowFileNotFound()
		{
			Action importLocations = () => new ImportLocationService().ImportLocations("");

			importLocations.Should().ThrowExactly<FileNotFoundException>();
		}

		[Fact]
		public void ImportLocations_FileWithOneLocation_ParseItOut()
		{
			Action importOneLocation = () => new ImportLocationService()
				.ImportLocations(@".\Data\OneLocation.json");

			importOneLocation.Should().ThrowExactly<JsonSerializationException>("we expect an array of locations while have just one item here");
		}

		[InlineData(@".\Data\TwoLocations.json")]
		[InlineData(@".\Data\TwoLocationsComaAtEnd.json")]
		[Theory]
		public void ImportLocations_FileWithTwoLocations_ParseTwoItemsOut(string filePath)
		{
			var discovered = new ImportLocationService().ImportLocations(filePath);

			discovered.Should().HaveCount(2);
		}

		[Fact(Skip = "large amount of work")]
		public void ImportLocations_BigFile_ParseAll()
		{
			var discovered = new ImportLocationService()
							.ImportLocations(@".\Data\AllLocations.json");

			discovered.Should()
				.NotContainNulls()
				.And.OnlyHaveUniqueItems()
				.And.HaveCountGreaterThan(100);
		}
	}
}
