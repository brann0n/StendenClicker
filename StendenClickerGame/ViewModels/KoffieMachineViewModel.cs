using StendenClicker.Library.Abilities;
using StendenClicker.Library.AbstractMonster;
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
		private List<SpecialAbility> UnlockedAbilities;
		public ObservableCollection<abilities> AbilitiesList { get; set; }
		public KoffieMachineViewModel()
		{
			AbilitiesList = new ObservableCollection<abilities>()
			{
				new abilities { AbilitieName = "Nucleare Koffie", AbilitieDescription = "Deze koffie bezorgt een harde klap aan de volgende Boss (Stackable)" , Cooldown = 60, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(KoffieAbilityClick) },
				new abilities { AbilitieName = "Gekoeld Water", AbilitieDescription = "", Cooldown = 120, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(WaterAbilityCommand) }
			};
		}

        private void WaterAbilityCommand()
        {
			CurrencyTrayViewModel.OnClickAbilityProcess += WaterAbility;
			Task.Delay(5000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= WaterAbility;
		}

        private void WaterAbility(object sender, EventArgs e)
        {
			
        }

        private void KoffieAbilityClick()
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += KoffieAbility;
		}

		private void KoffieAbility(object sender, System.EventArgs e)
		{
			GamePlatform platform = (GamePlatform)sender;

			AbstractMonster m = (AbstractMonster)platform.Monster;
			if(platform.Monster is Boss)
			{
				//is boss
				m.DoDamage(m.Health / 2);
				CurrencyTrayViewModel.OnClickAbilityProcess -= KoffieAbility;
            }
		}
	}
}

