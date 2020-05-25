using Killspiel.Settings.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Killspiel.Settings
{
	public class Settings
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public Messages Messages { get; set; } = new Messages();

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public Twitch Twitch { get; set; } = new Twitch();

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public League League { get; set; } = new League();

		private static readonly FileInfo SettingsFile = new FileInfo("settings/settings.json");

		public static Settings Instance { get; private set; } = default;

		private static Settings CompareInstance { get; set; } = default;

		public static async Task Load()
		{
			SettingsFile.Directory.Create();

			if (!SettingsFile.Exists)
				await CreateDefault();

			using (StreamReader streamReader = new StreamReader(new FileStream(SettingsFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				Settings loadedSettings = JsonConvert.DeserializeObject<Settings>(await streamReader.ReadToEndAsync());

				if (loadedSettings.Messages is null)
					loadedSettings.Messages = new Messages();

				if (loadedSettings.Twitch is null)
					loadedSettings.Twitch = new Twitch();

				if (loadedSettings.League is null)
					loadedSettings.League = new League();

				if (CheckForChanges(loadedSettings))
					await Save(loadedSettings);

				Instance = loadedSettings;
				CompareInstance = loadedSettings;
			}
		}

		public static async Task CreateDefault()
		{
			Settings defaultSettings = new Settings()
			{
				League = new League(),
				Twitch = new Twitch(),
				Messages = new Messages()
			};

			await Save(defaultSettings);
		}

		public static async Task Save(Settings instance)
		{
			StreamWriter configFileStream;

			SettingsFile.Directory.Create();

			if (!SettingsFile.Exists)
				configFileStream = new StreamWriter(SettingsFile.Create());
			else
				configFileStream = new StreamWriter(new FileStream(SettingsFile.FullName, FileMode.Truncate, FileAccess.Write, FileShare.None));

			using (configFileStream)
			{
				await configFileStream.WriteAsync(JsonConvert.SerializeObject(instance, Formatting.Indented));
				await configFileStream.FlushAsync();
			}
		}

		private static bool CheckForChanges(Settings oldInstance)
		{
			if (CompareInstance is null)
				return false;

			if (CompareInstance.Equals(oldInstance))
				return false;

			return true;
		}
	}
}