using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Factory;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StendenClickerGame.ViewModels
{
	public class KoffieMachineViewModel : ViewModelBase
	{
		private static ObservableCollection<Abilities> _AbilitiesList { get; set; }
		public ObservableCollection<Abilities> AbilitiesList { get => _AbilitiesList; set => _AbilitiesList = value; }
		public KoffieMachineViewModel()
		{
			var GerjanAbility = new Abilities
			{
				AbilitieName = "Gerjan's Tarwesmoothie",
				AbilitieDescription = "Deze hakt er hard in voor de volgende Boss! (Stackable)",
				IsOffCooldown = true,
				Image = "Assets/koffie.png"
			};
			GerjanAbility.OnExecute = new RelayFunctionCommand<Abilities>(GerjanSmoothieAbilityClick, GerjanAbility);

			var SjiAbility = new Abilities
			{
				AbilitieName = "Sji's Power Koffie",
				AbilitieDescription = "Dubbel de caffeïne (+1 Damage multi, 5s)",
				IsOffCooldown = true,
				Image = "Assets/koffie.png"
			};
			SjiAbility.OnExecute = new RelayFunctionCommand<Abilities>(SjiKoffieAbilityClick, SjiAbility);


			var JanAbility = new Abilities
			{
				AbilitieName = "Jan's Spa Bloedrood",
				AbilitieDescription = "Je vijand sparkelt uit elkaar!",
				IsOffCooldown = true,
				Image = "Assets/koffie.png",
			};
			JanAbility.OnExecute = new RelayFunctionCommand<Abilities>(JanWaterAbilityClick, JanAbility);
			
			var MiguelAbility = new Abilities
			{
				AbilitieName = "Miguel's Bubbel Thee",
				AbilitieDescription = "Kalmerende thee voor extra focus (+2 Damage multi, 10s)",
				IsOffCooldown = true,
				Image = "Assets/koffie.png",
			};
			MiguelAbility.OnExecute = new RelayFunctionCommand<Abilities>(MiguelTheeAbilityClick, MiguelAbility);

			var MartijnAbility = new Abilities
			{
				AbilitieName = "Martijn's Puur Suikerwater",
				AbilitieDescription = "WOOOOOOOOOOOOOO (+4 Damage multi, 15s)",
				IsOffCooldown = true,
				Image = "Assets/koffie.png",
			};
			MartijnAbility.OnExecute = new RelayFunctionCommand<Abilities>(MartijnSportAbilityClick, MartijnAbility);

			AbilitiesList = new ObservableCollection<Abilities>()
			{
				GerjanAbility,
				SjiAbility,
				JanAbility,
				MiguelAbility,
				MartijnAbility
			};
		}

        private async void MartijnSportAbilityClick(Abilities SelfContext)
        {
			ContextSetAbilityEnabled(SelfContext);

			CurrencyTrayViewModel.OnClickAbilityProcess += MartijnSportAbility;

			await ContextDelayProgressbarEmpty(SelfContext, 15000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= MartijnSportAbility;
			await ContextDelayProgressbarFill(SelfContext, 285000);

			ContextSetAbilityDisabled(SelfContext);
		}

        private void MartijnSportAbility(object sender, EventArgs e)
        {
			CurrencyTrayViewModel.AbilityMultiplier += 4;
		}

        private async void MiguelTheeAbilityClick(Abilities SelfContext)
		{
			ContextSetAbilityEnabled(SelfContext);

			CurrencyTrayViewModel.OnClickAbilityProcess += MiguelTheeAbility;

			await ContextDelayProgressbarEmpty(SelfContext, 10000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= MiguelTheeAbility;
			await ContextDelayProgressbarFill(SelfContext, 230000);

			ContextSetAbilityDisabled(SelfContext);
		}

		private void MiguelTheeAbility(object sender, EventArgs e)
		{
			CurrencyTrayViewModel.AbilityMultiplier += 2;
		}

		private async void JanWaterAbilityClick(Abilities SelfContext)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += JanWaterAbility;

			ContextSetAbilityEnabled(SelfContext);

			await ContextDelayProgressbarFill(SelfContext, 300000);

			ContextSetAbilityDisabled(SelfContext);
		}

		private void JanWaterAbility(object sender, EventArgs e)
		{
			GamePlatform platform = (GamePlatform)sender;
			AbstractMonster m = (AbstractMonster)platform.Monster;
			m.DoDamage(m.Health);
			CurrencyTrayViewModel.OnClickAbilityProcess -= JanWaterAbility;
		}

		private async void SjiKoffieAbilityClick(Abilities SelfContext)
		{
			ContextSetAbilityEnabled(SelfContext);

			CurrencyTrayViewModel.OnClickAbilityProcess += SjiKoffieAbility;

			await ContextDelayProgressbarEmpty(SelfContext, 5000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= SjiKoffieAbility;
			await ContextDelayProgressbarFill(SelfContext, 115000);

			ContextSetAbilityDisabled(SelfContext);
		}

		private void SjiKoffieAbility(object sender, EventArgs e)
		{
			CurrencyTrayViewModel.AbilityMultiplier += 1;
		}

		private async void GerjanSmoothieAbilityClick(Abilities SelfContext)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += GerjanSmoothieAbility;

			ContextSetAbilityEnabled(SelfContext);	
			
			await ContextDelayProgressbarFill(SelfContext, 150000);

			ContextSetAbilityDisabled(SelfContext);
		}		

		private void GerjanSmoothieAbility(object sender, System.EventArgs e)
		{
			GamePlatform platform = (GamePlatform)sender;

			AbstractMonster m = (AbstractMonster)platform.Monster;
			if (platform.Monster is Boss)
			{
				//is boss
				m.DoDamage(m.Health / 2);
				CurrencyTrayViewModel.OnClickAbilityProcess -= GerjanSmoothieAbility;
			}
		}


		private void ContextSetAbilityEnabled(Abilities SelfContext)
		{
			SelfContext.IsOffCooldown = false;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			SelfContext.NotifyPropertyChanged("IsCooldownProgressEnabled");
		}

		private void ContextSetAbilityDisabled(Abilities SelfContext)
		{
			SelfContext.IsOffCooldown = true;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			SelfContext.NotifyPropertyChanged("IsCooldownProgressEnabled");
		}

		private async Task ContextDelayProgressbarFill(Abilities SelfContext, int delayTime)
		{
			//devide delaytime by 500 to update the bar every half a second
			double amountOfTicks = delayTime / 100d;
			SelfContext.foreground = new SolidColorBrush(Colors.Silver);
			SelfContext.NotifyPropertyChanged("foreground");
			for (int i = 0; i < amountOfTicks; i++)
			{
				int percentage = (int)(i / amountOfTicks * 100d);
				TimeSpan ts = TimeSpan.FromSeconds(Math.Ceiling((amountOfTicks - i) / 10));
				SelfContext.CooldownPercentage = percentage;
				SelfContext.CooldownTime = ts;
				SelfContext.NotifyPropertyChanged("CooldownPercentage");
				SelfContext.NotifyPropertyChanged("CooldownTime");
				await Task.Delay(100);
			}
		}

		private async Task ContextDelayProgressbarEmpty(Abilities SelfContext, int delayTime)
		{
			//devide delaytime by 500 to update the bar every half a second
			double amountOfTicks = delayTime / 100d;
			SelfContext.IsCooldownTimerEnabled = false;
			SelfContext.NotifyPropertyChanged("IsCooldownTimerEnabled");
			SelfContext.foreground = new SolidColorBrush(Colors.Red);
			SelfContext.NotifyPropertyChanged("foreground");
			for (int i = (int)amountOfTicks; i >= 0; i--)
			{
				int percentage = (int)(i / amountOfTicks * 100d);
				SelfContext.CooldownPercentage = percentage;
				SelfContext.NotifyPropertyChanged("CooldownPercentage");
				await Task.Delay(100);
			}
			SelfContext.IsCooldownTimerEnabled = true;
			SelfContext.NotifyPropertyChanged("IsCooldownTimerEnabled");
		}
	}
}

