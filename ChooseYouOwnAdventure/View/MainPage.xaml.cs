using System;
using System.Windows.Input;
using ChooseYouOwnAdventure.Model;
using ChooseYouOwnAdventure.View;
using ChooseYouOwnAdventure.ViewModel;
using Ink.Runtime;

namespace ChooseYouOwnAdventure.View;

public partial class MainPage : ContentPage
{
	public MainPage(StoriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}


