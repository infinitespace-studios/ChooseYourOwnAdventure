using System;
using System.Windows.Input;
using ChooseYourOwnAdventure.Model;
using ChooseYourOwnAdventure.View;
using ChooseYourOwnAdventure.ViewModel;
using Ink.Runtime;

namespace ChooseYourOwnAdventure.View;

public partial class MainPage : ContentPage
{
	public MainPage(StoriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}


