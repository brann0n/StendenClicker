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
				new abilities { AbilitieName = "Gerjan's Nucleare Koffie", AbilitieDescription = "Bezorgt een harde klap aan de volgende Boss (Stackable)" , Cooldown = 60, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(GerjanKoffieAbilityClick) },
				new abilities { AbilitieName = "Sji's Gekoeld Water", AbilitieDescription = "Dubbel zo verfrissend, Dubbel de damage (5s)", Cooldown = 120, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(SjiWaterAbilityCommandAsync) }
			};
		}

        private async void SjiWaterAbilityCommandAsync()
        {
			await Task.Run(async () => //Task.Run automatically unwraps nested Task types!
			{
				CurrencyTrayViewModel.OnClickAbilityProcess += SjiWaterAbility;
				await Task.Delay(5000);
				CurrencyTrayViewModel.OnClickAbilityProcess -= SjiWaterAbility;
			});
		}

        private void SjiWaterAbility(object sender, EventArgs e)
        {
			GamePlatform platform = (GamePlatform)sender;

			AbstractMonster m = (AbstractMonster)platform.Monster;
			if (platform.Monster is Boss)
			{
				//is boss
				m.DoDamage(m.Health / 2);
			}
		}

        private void GerjanKoffieAbilityClick()
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += GerjanKoffieAbility;
		}

		private void GerjanKoffieAbility(object sender, System.EventArgs e)
		{
			GamePlatform platform = (GamePlatform)sender;

			AbstractMonster m = (AbstractMonster)platform.Monster;
			if(platform.Monster is Boss)
			{
				//is boss
				m.DoDamage(m.Health / 2);
				CurrencyTrayViewModel.OnClickAbilityProcess -= GerjanKoffieAbility;
            }
		}
	}
}

