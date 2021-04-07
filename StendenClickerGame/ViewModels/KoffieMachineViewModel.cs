using StendenClicker.Library.Abilities;
using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Factory;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
				AbilitieDescription = "Bezorgt een harde klap aan de volgende Boss! (Stackable)",
				IsOffCooldown = true,
				Image = "Assets/koffie.png"
			};
			GerjanAbility.OnExecute = new RelayFunctionCommand<Abilities>(GerjanSmoothieAbilityClick, GerjanAbility);

			var SjiAbility = new Abilities
			{
				AbilitieName = "Sji's Power Koffie",
				AbilitieDescription = "Dubbel de caffeïne, Dubbel de damage! (5s)",
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

			AbilitiesList = new ObservableCollection<Abilities>()
			{
				GerjanAbility,
				SjiAbility,
				JanAbility
			};
		}

		private async void JanWaterAbilityClick(Abilities SelfContext)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += JanWaterAbility;
			SelfContext.IsOffCooldown = false;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			await Task.Delay(300000);
			SelfContext.IsOffCooldown = true;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
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
			SelfContext.IsOffCooldown = false;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			CurrencyTrayViewModel.OnClickAbilityProcess += SjiKoffieAbility;
			await Task.Delay(5000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= SjiKoffieAbility;
			await Task.Delay(115000);
			SelfContext.IsOffCooldown = true;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
		}

		private void SjiKoffieAbility(object sender, EventArgs e)
		{
			CurrencyTrayViewModel.AbilityMultiplier = 2;
		}

		private async void GerjanSmoothieAbilityClick(Abilities SelfContext)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += GerjanSmoothieAbility;
			SelfContext.IsOffCooldown = false;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");	
			SelfContext.NotifyPropertyChanged("IsCooldownProgressEnabled");			
			await DoDelayWithProgressbar(SelfContext, 150000);
			SelfContext.IsOffCooldown = true;
			SelfContext.NotifyPropertyChanged("IsOffCooldown");
			SelfContext.NotifyPropertyChanged("IsCooldownProgressEnabled");
		}

		private async Task DoDelayWithProgressbar(Abilities SelfContext, int delayTime)
		{
			//devide delaytime by 500 to update the bar every half a second
			double amountOfTicks = delayTime / 100d;
			for (int i = 0; i < amountOfTicks; i++)
			{
				int percentage = (int)(i / amountOfTicks * 100d);
				SelfContext.CooldownPercentage = percentage;
				SelfContext.NotifyPropertyChanged("CooldownPercentage");
				await Task.Delay(100);
			}
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
	}
}

