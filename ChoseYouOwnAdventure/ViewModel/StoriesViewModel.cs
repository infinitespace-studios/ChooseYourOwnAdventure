using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;
using ChoseYouOwnAdventure.Service;
using ChoseYouOwnAdventure.View;

namespace ChoseYouOwnAdventure.ViewModel
{
	public class StoriesViewModel : BaseViewModel
	{
		StoryService storyService;
		IConnectivity connectivity;
		public ObservableCollection<StoryEntry> Stories { get; } = new ObservableCollection<StoryEntry>();
		public ICommand StorySelected { get; private set; }
		public ICommand GetStories { get; private set; }


		public StoriesViewModel(StoryService service, IConnectivity connectivity)
		{
			this.connectivity = connectivity;
			storyService = service;
			Title = "Stories";
			StorySelected = new Command<StoryEntry>(async (s) => {
				await SelectStorEntry(s);
			});
			GetStories = new Command(async () => { await GetStoriesAsync(); });
		}

		async Task SelectStorEntry (StoryEntry entry)
		{
			if (entry is null)
				return;

			await Shell.Current.GoToAsync($"{nameof (StoryView)}", animate: true, parameters: new Dictionary<string, object>() {
				{"StoryEntry", entry },
			});
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

