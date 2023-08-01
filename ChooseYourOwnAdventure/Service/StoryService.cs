using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using ChooseYourOwnAdventure.Model;

namespace ChooseYourOwnAdventure.Service
{
	public class StoryService
	{
		const string DEFAULT_MANIFEST = "Resources/Raw/Stories.json";
		//const string ROOT_URL = "https://raw.githubusercontent.com/infinitespace-studios/ChooseYourOwnAdventure/development/ChoseYouOwnAdventure";
		const string ROOT_URL = "https://raw.githubusercontent.com/infinitespace-studios/ChooseYourOwnAdventureStories/main";

		HttpClient client;
		List<StoryEntry> storyEntries;
		IConnectivity connectivity;

		public StoryService(IConnectivity connectivity)
		{
			this.connectivity = connectivity;
			client = new HttpClient();
		}
		public async Task<List<StoryEntry>> GetStories (bool forcerefresh = false)
		{
			if ((storyEntries?.Count ?? 0) > 0 && !forcerefresh)
				return storyEntries;

			string url = $"{ROOT_URL}/{DEFAULT_MANIFEST}";

			HttpResponseMessage response = null;
			if (connectivity.NetworkAccess == NetworkAccess.Internet) {
				response = await client.GetAsync(url);
			}
			if (response is null || !response.IsSuccessStatusCode)
			{
				// load backup data.
				if (!await FileSystem.Current.AppPackageFileExistsAsync(DEFAULT_MANIFEST))
				{
					return new List<StoryEntry> ();
				}
				using var stream = await FileSystem.Current.OpenAppPackageFileAsync(DEFAULT_MANIFEST);

				storyEntries = await JsonSerializer.DeserializeAsync<List<StoryEntry>>(stream);
				return storyEntries;
			}
			storyEntries = await JsonSerializer.DeserializeAsync<List<StoryEntry>>(response.Content.ReadAsStream());
			return storyEntries;
		}

		public async Task<Ink.Runtime.Story> GetStory (StoryEntry entry)
		{
			string json = string.Empty;
			string path = Path.Combine(FileSystem.AppDataDirectory, this.GetType ().Assembly.GetName ().Name, entry.StoryFile);
			if (await FileSystem.Current.AppPackageFileExistsAsync(entry.StoryFile))
			{
				// we have a local copy. Load it.
				using var stream = await FileSystem.Current.OpenAppPackageFileAsync(entry.StoryFile);
				using var sr = new StreamReader(stream);
				json = await sr.ReadToEndAsync();
			}
			if (string.IsNullOrEmpty (json) && File.Exists (path))
			{
				json = File.ReadAllText(path);
			}
			if (string.IsNullOrEmpty(json) && connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				var response = await client.GetAsync($"{ROOT_URL}/{entry.StoryFile}");
				if (response.IsSuccessStatusCode)
				{
					json = await response.Content.ReadAsStringAsync();
					try
					{
						Directory.CreateDirectory(Path.GetDirectoryName(path));
						File.WriteAllText(path, json);
					} catch (IOException)
					{
						// oops
					}
				}
			}
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			var story = new Ink.Runtime.Story(json);
			return story;
		}


	}
}

