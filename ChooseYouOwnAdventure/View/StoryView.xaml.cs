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

		private async void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsChoosing")
			{
				if (vm.IsChoosing)
				{
					try
					{
						story.ScrollTo(vm.Lines.Count() - 1);
					} catch
					{
						// If we fail ignore it.
					}
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
