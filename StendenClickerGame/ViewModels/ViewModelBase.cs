using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StendenClickerGame.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// The important notifier method of changed properties. This function should be called whenever you want to inform other classes that some property has changed.
		/// </summary>
		/// <param name="propertyName">The name of the updated property. Leaving this blank will fill in the name of the calling property.</param>
		protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
