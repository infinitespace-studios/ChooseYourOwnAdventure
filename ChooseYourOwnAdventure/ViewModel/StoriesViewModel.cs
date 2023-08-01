using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using ChooseYourOwnAdventure.Model;
using ChooseYourOwnAdventure.Service;
using ChooseYourOwnAdventure.View;

namespace ChooseYourOwnAdventure.ViewModel
{
	public class StoriesViewModel : BaseViewModel
	{
		StoryService storyService;
		IConnectivity connectivity;
		public ObservableCollection<StoryEntry> Stories { get; } = new ObservableCollection<StoryEntry>();
		public ICommand StorySelected { get; private set; }
		public ICommand GetStories { get; private set; }
		public bool IsRefreshing { get; set; }

		public StoriesViewModel(StoryService service, IConnectivity connectivity)
		{
			this.connectivity = connectivity;
			storyService = service;
			Title = "Stories";
			StorySelected = new Command<StoryEntry>(async (s) => {
				await SelectStoryEntry(s);
			});
			GetStories = new Command(async () => {
				await GetStoriesAsync();
				IsRefreshing = false;
				OnPropertyChanged(nameof(IsRefreshing));
			});
			Shell.Current.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds (500),async () => {
				await GetStoriesAsync(forceRefresh: false);
			});
		}

		async Task SelectStoryEntry (StoryEntry entry)
		{
			if (entry is null)
				return;

			try
			{
				await Shell.Current.GoToAsync($"{nameof(StoryView)}", animate: true, parameters: new Dictionary<string, object>() {
				{"StoryEntry", entry },
			});
			} catch (Exception ex)
			{
				Console.WriteLine (ex);
				throw;
			}
		}

		async Task GetStoriesAsync(bool forceRefresh = true)
		{
			if (IsBusy)
				return;

			try
			{
				IsBusy = true;
				var stories = await storyService.GetStories(forcerefresh: forceRefresh);
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

