using System;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;
using ChoseYouOwnAdventure.View;
using ChoseYouOwnAdventure.ViewModel;
using Ink.Runtime;

namespace ChoseYouOwnAdventure;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Dispatcher.Dispatch(async () => {
			bool result = await model.LoadStories();
			System.Diagnostics.Debug.WriteLine($"LoadStories: {result}");
		});
	}

	void ListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
	{
		StoryEntry entry = (StoryEntry)e.Item;
		Navigation.PushAsync(new StoryView(entry));
	}

}


