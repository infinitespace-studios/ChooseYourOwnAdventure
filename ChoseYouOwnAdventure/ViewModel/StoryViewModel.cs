using System;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;
using Ink.Runtime;

namespace ChoseYouOwnAdventure.ViewModel
{
	public class StoryViewModel : BaseViewModel
	{
		public Story Story { get; private set; }

		public IEnumerable<Line> Lines => GetLines();

		public IEnumerable<Choice> Choices => GetChoices();
		public ICommand Choose { get; private set; }

		public async void Init(StoryEntry story)
		{
			this.Story = await LoadStory (story);
			Choose = new Command<Choice>((c) => {
				Story.ChooseChoiceIndex(c.index);
			});
		}

		private async Task<Story> LoadStory(StoryEntry story)
		{
			using var stream = await FileSystem.Current.OpenAppPackageFileAsync(story.StoryFile);
			using var sr = new StreamReader(stream);
			string json = sr.ReadToEnd();
			return new Story(json);
		}

		IEnumerable<Line> GetLines()
		{
			while (Story.canContinue)
			{
				yield return new Line() { Text = Story.Continue(), Image = GetImageTag () };
			}
		}

		IEnumerable<Choice> GetChoices()
		{

			foreach (var choice in Story.currentChoices)
			{
				yield return choice;
			}
		}

		string GetImageTag ()
		{
			foreach (var tag in Story.currentTags)
			{
				if (tag.StartsWith ("image:"))
				{
					return tag.Replace("image:", string.Empty).Trim();
				}
			}
			return String.Empty;
		}

	}
}

