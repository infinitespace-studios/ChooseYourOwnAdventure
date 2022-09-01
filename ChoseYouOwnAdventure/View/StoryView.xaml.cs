using System;
using ChoseYouOwnAdventure.Model;
using ChoseYouOwnAdventure.ViewModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace ChoseYouOwnAdventure.View
{
	public partial class StoryView : ContentPage
	{
		public StoryViewModel model;
		public StoryView(StoryEntry entry)
		{
			InitializeComponent();
			model = new StoryViewModel();
			model.Init(entry);
		}
	}
}
