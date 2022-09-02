using System;
using System.Windows.Input;
usingChooseYourOwnAdventure.Model;
usingChooseYourOwnAdventure.View;
usingChooseYourOwnAdventure.ViewModel;
using Ink.Runtime;

namespaceChooseYourOwnAdventure.View;

public partial class MainPage : ContentPage
{
	public MainPage(StoriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}


