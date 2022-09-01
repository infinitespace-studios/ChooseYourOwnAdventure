using System;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;

namespace ChoseYouOwnAdventure.ViewModel
{
	public class StoriesViewModel : BaseViewModel
	{
		const string DEFAULT_MANIFEST = "Resources/Raw/Stories.json";
		public List<StoryEntry> Stories => stories;

		public ICommand StorySelected { get; private set; }

		List<StoryEntry> stories = new List<StoryEntry>();

		public StoriesViewModel()
		{
			StorySelected = new Command<StoryEntry>((s) => {
                System.Diagnostics.Debug.WriteLine($"Selected {s.Name}");
            });
		}

		public async Task<bool> LoadStories ()
		{
			if (!await FileSystem.Current.AppPackageFileExistsAsync(DEFAULT_MANIFEST))
			{
				return false;
			}
			using var stream = await FileSystem.Current.OpenAppPackageFileAsync(DEFAULT_MANIFEST);

			var s1 = await JsonSerializer.DeserializeAsync<List<StoryEntry>>(stream);
			foreach (var s in s1)
			{
				System.Diagnostics.Debug.WriteLine($"Adding {s.Name}");
				stories.Add(s);
				System.Diagnostics.Debug.WriteLine($"Stories {stories.Count ()}");
			}
			OnPropertyChanged(nameof(Stories));
			return true;
		}
	}
}

