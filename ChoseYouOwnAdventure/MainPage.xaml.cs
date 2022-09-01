using System;
using System.Windows.Input;
using ChoseYouOwnAdventure.Model;
using ChoseYouOwnAdventure.View;
using ChoseYouOwnAdventure.ViewModel;
using Ink.Runtime;

namespace ChoseYouOwnAdventure.View;

public partial class MainPage : ContentPage
{
	public MainPage(StoriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}


