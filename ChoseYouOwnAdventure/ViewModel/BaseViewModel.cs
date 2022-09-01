using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChoseYouOwnAdventure.ViewModel
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		bool isBusy;

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

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

