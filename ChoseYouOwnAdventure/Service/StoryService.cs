using System;
using System.Text.Json;
using System.Net.Http;
using ChoseYouOwnAdventure.Model;

namespace ChoseYouOwnAdventure.Service
{
	public class StoryService
	{
		const string DEFAULT_MANIFEST = "Resources/Raw/Stories.json";

		HttpClient client;
		List<StoryEntry> storyEntries;

		public StoryService()
		{
			client = new HttpClient();
		}
		public async Task<List<StoryEntry>> GetStories ()
		{
			if ((storyEntries?.Count ?? 0) > 0)
				return storyEntries;

			string url = $"https://raw.githubusercontent.com/infinitespace-studios/ChooseYouOwnAdventure/development/ChoseYouOwnAdventure/${DEFAULT_MANIFEST}";
			var response = await client.GetAsync(url);
			if (!response.IsSuccessStatusCode)
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


	}
}

