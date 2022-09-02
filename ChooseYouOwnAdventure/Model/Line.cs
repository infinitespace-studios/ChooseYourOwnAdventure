using System;
namespace ChooseYouOwnAdventure.Model
{
	public class Line
	{
		public string Text { get; set; }
		public string Image { get; set; }

		public bool HasText => !string.IsNullOrEmpty(Text);
		public bool HasImage => !string.IsNullOrEmpty(Image);
	}
}

