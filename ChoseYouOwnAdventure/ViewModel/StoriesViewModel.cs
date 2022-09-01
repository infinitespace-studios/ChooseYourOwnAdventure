using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;
using ChoseYouOwnAdventure.Service;

namespace ChoseYouOwnAdventure.ViewModel
{
	public class StoriesViewModel : BaseViewModel
	{
		StoryService storyService;
		public ObservableCollection<StoryEntry> Stories { get; } = new ObservableCollection<StoryEntry>();
		public ICommand StorySelected { get; private set; }
		public ICommand GetStories { get; private set; }


		public StoriesViewModel(StoryService service)
		{
			storyService = service;
			Title = "Stories";
			StorySelected = new Command<StoryEntry>((s) => {
				System.Diagnostics.Debug.WriteLine($"Selected {s.Name}");
			});
			GetStories = new Command(async () => { await GetStoriesAsync(); });
		}

		async Task GetStoriesAsync()
		{
			if (IsBusy)
				return;

			try
			{
				IsBusy = true;
				var stories = await storyService.GetStories();
				if (Stories.Count > 0)
					Stories.Clear();
				foreach (var story in stories) {
					Stories.Add(story);
				}

			} catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			} finally
			{
				IsBusy = false;
			}
		}
	}
}

