using System;
using ChooseYourOwnAdventure.Model;
using ChooseYourOwnAdventure.ViewModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using CommunityToolkit.Maui.Views;

namespace ChooseYourOwnAdventure.View
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
			choices.PropertyChanged += Choices_PropertyChanged;
		}

		private void Choices_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsVisible")
			{
				if (choices.IsVisible)
				{
					try
					{
						Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds (50), () => {
							story.ScrollTo(vm.Lines.Count() - 1);
						});
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex);						// If we fail ignore it.
					}
				}
			}
		}

		private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsChoosing")
			{
				if (vm.IsChoosing)
				{
					try
					{
					//	story.ScrollTo(vm.Lines.Count() - 1);
					} catch
					{
						// If we fail ignore it.
					}
				} else
				{
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
