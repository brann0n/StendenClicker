using StendenClicker.Library.Models.DatabaseModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HeroObject = StendenClicker.Library.PlayerControls.Hero;

namespace StendenClickerGame.Data
{
	public class HeroListObject : INotifyPropertyChanged
	{
		public HeroObject Hero { get; set; }
		public PlayerHero PlayerHeroInformation { get; set; }

		public bool HeroUnlocked { get; set; } //can be calculated that after a certain boss has been defeated, unlock this.
		public bool HeroBought { get => PlayerHeroInformation != null; } //if true the buy button should change into upgrade button.

		public double OpacityEnabled { get => HeroUnlocked ? 1 : 0.3; }

		public ulong NextUpgradePriceSparkCoins { get; set; } = 1000;
		public int UpgradePriceEuropeanCredits { get; set; } = 15;

		public ICommand OnHeroButtonClicked { get; set; }
		public ICommand OnHeroButtonClickSpecialUpgrade { get; set; }

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
