﻿using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using ChoseYouOwnAdventure.Model;

namespace ChoseYouOwnAdventure.Service
{
	public class StoryService
	{
		const string DEFAULT_MANIFEST = "Resources/Raw/Stories.json";
		const string ROOT_URL = "https://raw.githubusercontent.com/infinitespace-studios/ChooseYouOwnAdventure/development/ChoseYouOwnAdventure";

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

			string url = $"{ROOT_URL}/${DEFAULT_MANIFEST}";
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

		public async Task<Ink.Runtime.Story> GetStory (StoryEntry entry)
		{
			string json = string.Empty;
			string path = Path.Combine(FileSystem.AppDataDirectory, entry.StoryFile);
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
			if (string.IsNullOrEmpty(json))
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

