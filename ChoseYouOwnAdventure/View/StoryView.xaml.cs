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
		StoryViewModel vm;
		public StoryView(StoryViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
			vm = viewModel;
			vm.PropertyChanged += Vm_PropertyChanged;
		}

		private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsChoosing")
			{
				if (vm.IsChoosing)
				{
					//await Choices.ScaleTo(0.0, 0, Easing.Linear);
					//await Choices.ScaleTo(1.0, 250, Easing.BounceIn);

				} else
				{
					//await Choices.ScaleTo(0.0, 250, Easing.BounceOut);
				}
			}
		}

		protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
		{
			// We are moving away from this page.
			vm.Closing();
			base.OnNavigatingFrom(args);
		}
	}
}
