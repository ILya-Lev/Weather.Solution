using Newtonsoft.Json;
using OpenWeatherMap.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace OpenWeatherMap.DataAccess
{
	public class ImportLocationService
	{
		private static readonly Dictionary<Func<string, bool>, Func<string, IEnumerable<Location>>> _fileReaders = new Dictionary<Func<string, bool>, Func<string, IEnumerable<Location>>>
		{
			[HasExtension(".json")] = ReadJsonFile,
			[HasExtension(".txt")] = ReadJsonFile,
			[HasExtension(".zip")] = ReadZipFile,
			[HasExtension(".gz")] = ReadGZipFile,
			[HasExtension(".7z")] = Read7ZipFile,

		};

		public IEnumerable<LocationDb> ImportLocations(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Cannot find file with supported locations data.", filePath);
			}

			var locations = _fileReaders.FirstOrDefault(pair => pair.Key(filePath)).Value(filePath);

			var locationsDb = locations
			.Where(loc => loc?.CountryCode?.Length == 2)
			.Select(loc => new LocationDb
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

		private static Func<string, bool> HasExtension(string extension)
		{
			return filePath => filePath?.EndsWith(extension, StringComparison.OrdinalIgnoreCase) ?? false;
		}

		private static IEnumerable<Location> ReadJsonFile(string filePath)
		{
			using (var openFile = File.Open(filePath, FileMode.Open))
				return DeserializeFromStream(openFile);
		}

		private static IEnumerable<Location> ReadGZipFile(string filePath)
		{
			using (var openFile = File.Open(filePath, FileMode.Open))
			using (var decompress = new GZipStream(openFile, System.IO.Compression.CompressionMode.Decompress))
				return DeserializeFromStream(decompress);
		}

		private static IEnumerable<Location> Read7ZipFile(string filePath)
		{
			throw new InvalidOperationException($"7zip files are not supported");
		}

		private static IEnumerable<Location> ReadZipFile(string filePath)
		{
			var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			try
			{
				var tempDirectory = Directory.CreateDirectory(tempDirectoryPath);
				ZipFile.ExtractToDirectory(filePath, tempDirectoryPath);
				var unzippedFiles = tempDirectory.GetFiles();

				if (!unzippedFiles.Any())
					throw new FileNotFoundException("There is nothing to read from temporary folder.", tempDirectoryPath);

				using (var openFile = File.Open(unzippedFiles[0].FullName, FileMode.Open))
					return DeserializeFromStream(openFile);
			}
			finally
			{
				foreach (var file in Directory.EnumerateFiles(tempDirectoryPath))
				{
					File.Delete(file);
				}

				Directory.Delete(tempDirectoryPath);
			}
		}

		private static IEnumerable<Location> DeserializeFromStream(Stream inputStream)
		{
			using (var streamReader = new StreamReader(inputStream))
			using (var jsonReader = new JsonTextReader(streamReader))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<Location[]>(jsonReader);
			}
		}
	}
}
