using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StendenClickerGame.Data
{
	public class Abilities : INotifyPropertyChanged
	{
		public string AbilitieName { get; set; }
		public string AbilitieDescription { get; set; }
		public bool IsOffCooldown { get; set; }
		public string Image { get; set; }

		public Windows.UI.Xaml.Media.SolidColorBrush foreground { get; set; }

		public bool IsCooldownProgressEnabled { get => !IsOffCooldown; }
		public bool IsCooldownTimerEnabled { get; set; } = true;
		public int CooldownPercentage { get; set; }

		public TimeSpan CooldownTime { get; set; }

		public ICommand OnExecute { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// The important notifier method of changed properties. This function should be called whenever you want to inform other classes that some property has changed.
		/// </summary>
		/// <param name="propertyName">The name of the updated property. Leaving this blank will fill in the name of the calling property.</param>
		public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
