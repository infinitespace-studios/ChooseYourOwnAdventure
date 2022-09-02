using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChooseYourOwnAdventure.ViewModel
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		bool isBusy;
		string title;

		public event PropertyChangedEventHandler PropertyChanged;
		
		public bool IsBusy {
			get => isBusy;
			set
			{
				if (isBusy == value)
					return;
				isBusy = value;
				OnPropertyChanged();
			}
		}

		public bool IsNotBusy => !IsBusy;

		public string Title { get => title;
			set {
				if (title == value)
					return;
				title = value;
				OnPropertyChanged();
			}
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

