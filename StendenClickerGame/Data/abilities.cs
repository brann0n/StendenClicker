using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StendenClickerGame.Data
{
	public class Abilities : INotifyPropertyChanged
	{
		public string AbilitieName { get; set; }
		public string AbilitieDescription { get; set; }
		public bool IsOffCooldown { get; set; }
		public string Image { get; set; }

		public Windows.UI.Xaml.Media.SolidColorBrush Foreground { get; set; }

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

	public static class AbilitiesExtensions
	{
		private static readonly double ProgressbarTicks = 100d;

		private static double ToSecondConversionFactor { get { return 1000 / ProgressbarTicks; } }

		public static void ContextSetAbilityEnabled(this Abilities SelfContext)
		{
			SelfContext.IsOffCooldown = false;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			SelfContext.NotifyPropertyChanged("IsCooldownProgressEnabled");
		}

		public static void ContextSetAbilityDisabled(this Abilities SelfContext)
		{
			SelfContext.IsOffCooldown = true;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			SelfContext.NotifyPropertyChanged("IsCooldownProgressEnabled");
		}

		public static async Task ContextDelayProgressbarFill(this Abilities SelfContext, int delayTime)
		{
			//devide delaytime by ticks to update the bar
			double amountOfTicks = delayTime / ProgressbarTicks;
			SelfContext.Foreground = new SolidColorBrush(Colors.Silver);
			SelfContext.NotifyPropertyChanged("foreground");
			for (int i = 0; i < amountOfTicks; i++)
			{
				int percentage = (int)(i / amountOfTicks * ProgressbarTicks);
				TimeSpan ts = TimeSpan.FromSeconds(Math.Ceiling((amountOfTicks - i) / ToSecondConversionFactor));
				SelfContext.CooldownPercentage = percentage;
				SelfContext.CooldownTime = ts;
				SelfContext.NotifyPropertyChanged("CooldownPercentage");
				SelfContext.NotifyPropertyChanged("CooldownTime");
				await Task.Delay((int)ProgressbarTicks);
			}
		}

		public static async Task ContextDelayProgressbarEmpty(this Abilities SelfContext, int delayTime)
		{
			//devide delaytime by ticks to update the bar
			double amountOfTicks = delayTime / ProgressbarTicks;
			SelfContext.IsCooldownTimerEnabled = false;
			SelfContext.NotifyPropertyChanged("IsCooldownTimerEnabled");
			SelfContext.Foreground = new SolidColorBrush(Colors.Red);
			SelfContext.NotifyPropertyChanged("foreground");
			for (int i = (int)amountOfTicks; i >= 0; i--)
			{
				int percentage = (int)(i / amountOfTicks * ProgressbarTicks);
				SelfContext.CooldownPercentage = percentage;
				SelfContext.NotifyPropertyChanged("CooldownPercentage");
				await Task.Delay((int)ProgressbarTicks);
			}
			SelfContext.IsCooldownTimerEnabled = true;
			SelfContext.NotifyPropertyChanged("IsCooldownTimerEnabled");
		}
	}
}
