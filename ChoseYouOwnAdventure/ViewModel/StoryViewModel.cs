using System;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;
using ChoseYouOwnAdventure.Service;
using InkRuntime = Ink.Runtime;

namespace ChoseYouOwnAdventure.ViewModel
{
	[QueryProperty ("StoryEntry", "StoryEntry")]
	public class StoryViewModel : BaseViewModel
	{
		StoryService storyService;
		StoryEntry storyEntry;
		InkRuntime.Story story;
		bool isChoosing;
		public StoryEntry StoryEntry {
			get => storyEntry;
			set {
				if (value is null)
					return;
				storyEntry = value;
				App.Current.Dispatcher.Dispatch (async () => await LoadStory (storyEntry));
			}
		}

		public ObservableCollection<Line> Lines { get; } = new ObservableCollection<Line> ();

		public IEnumerable<InkRuntime.Choice> Choices => GetChoices();
		public bool IsChoosing {
			get => isChoosing;
			set {
				if (isChoosing == value)
					return;
				isChoosing = value;
				OnPropertyChanged();
			}
		}

		public bool IsComplete { get => !story?.canContinue ?? false; }

		public ICommand Choose { get; private set; }
		public ICommand Restart { get; private set; }

		public StoryViewModel (StoryService service)
		{
			storyService = service;
			Choose = new Command<InkRuntime.Choice>((c) => {
				IsBusy = true;
				try
				{
					if (c is null)
						return;
					IsChoosing = false;
					story.ChooseChoiceIndex(c.index);
					ReadLines();
					OnPropertyChanged(nameof(Choices));
				} finally
				{
					IsBusy = false;
				}
			});
			Restart = new Command(() => {
				IsBusy = true;
				try
				{
					story?.ResetState();
					Lines.Clear();
					RemoveSaveState();
					IsChoosing = false;
					ReadLines();
				} finally
				{
					IsBusy = false;
				}
			});
		}

		private async Task<bool> LoadStory(StoryEntry entry)
		{
			try
			{
				IsBusy = true;
				story = await storyService.GetStory(entry);
				LoadState();
				ReadLines();
				return true;
			} finally
			{
				IsBusy = false;
			}
		}

		void ReadLines ()
		{
			if (story is null)
			{
				OnPropertyChanged(nameof(IsComplete));
				return;
			}
			
			while (story.canContinue)
			{
					Lines.Add(new Line() { Text = story.Continue(), Image = GetImageTag() });
			}
			if (story.currentChoices.Count > 0)
			{
				OnPropertyChanged(nameof(Choices));
				IsChoosing = true;
			}
			OnPropertyChanged(nameof(IsComplete));
		}

		IEnumerable<InkRuntime.Choice> GetChoices()
		{
			if (story is null)
			{
				yield return null;
			}
			else
			{
				foreach (var choice in story.currentChoices)
				{
					yield return choice;
				}
			}
		}

		string GetImageTag ()
		{
			foreach (var tag in story.currentTags)
			{
				if (tag.StartsWith ("image:"))
				{
					return tag.Replace("image:", string.Empty).Trim();
				}
			}
			return String.Empty;
		}

		void SaveState ()
		{
			string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Saves", Path.GetFileName(storyEntry.StoryFile));
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			var lineData = JsonSerializer.Serialize<ObservableCollection<Line>>(Lines);
			File.WriteAllText(Path.ChangeExtension (path, "dat"), lineData);
			string state = story.state.ToJson();
			File.WriteAllText(path, state);
		}

		void RemoveSaveState ()
		{
			string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Saves", Path.GetFileName(storyEntry.StoryFile));
			if (File.Exists(path))
				File.Delete(path);
			if (File.Exists (Path.ChangeExtension(path, "dat")))
				File.Delete(Path.ChangeExtension(path, "dat"));
		}

		void LoadState ()
		{
			string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Saves", Path.GetFileName (storyEntry.StoryFile));
			if (!File.Exists(path))
				return;
			string lineData = Path.ChangeExtension(path, "dat");
			if (File.Exists(lineData))
			{
				var data = JsonSerializer.Deserialize<ObservableCollection<Line>>(File.ReadAllText(lineData));
				foreach (var l in data)
					Lines.Add(l);

			}	
			string json = File.ReadAllText(path);
			try
			{
				story.state.LoadJson(json);
			} catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}

		public void Closing()
		{
			// we are closing , we need to save the state of the story.
			SaveState();
		}
	}
}

