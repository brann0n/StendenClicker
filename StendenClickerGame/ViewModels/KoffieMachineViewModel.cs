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
		public ObservableCollection<Abilities> AbilitiesList { get; set; }
		public KoffieMachineViewModel()
		{
			AbilitiesList = new ObservableCollection<Abilities>()
			{
				new Abilities { AbilitieName = "Gerjan's Tarwesmoothie", AbilitieDescription = "Bezorgt een harde klap aan de volgende Boss (Stackable)" , Cooldown = 60, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(GerjanSmoothieAbilityClick) },
				new Abilities { AbilitieName = "Sji's Power Koffie", AbilitieDescription = "Dubbel de caffeïne, Dubbel de damage (5s)", Cooldown = 120, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(SjiKoffieAbilityClickAsync) },
				new Abilities { AbilitieName = "Jan's Spa Bloedrood", AbilitieDescription = "Leun achterover en laat het spel het werk doen (10s)", Cooldown = 300, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(JanWaterAbilityClick)}
			};
		}

        private void JanWaterAbilityClick()
        {
			CurrencyTrayViewModel.OnClickAbilityProcess += JanWaterAbility;
        }

        private void JanWaterAbility(object sender, EventArgs e)
        {
              
        }

        private async void SjiKoffieAbilityClickAsync()
        {
			await Task.Run(async () => //Task.Run automatically unwraps nested Task types!
			{
				CurrencyTrayViewModel.OnClickAbilityProcess += SjiKoffieAbility;
				await Task.Delay(5000);
				CurrencyTrayViewModel.OnClickAbilityProcess -= SjiKoffieAbility;
			});
		}

        private void SjiKoffieAbility(object sender, EventArgs e)
        {
			CurrencyTrayViewModel.AbilityMultiplier = 2;
		}

        private void GerjanSmoothieAbilityClick()
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += GerjanSmoothieAbility;
		}

		private void GerjanSmoothieAbility(object sender, System.EventArgs e)
		{
			GamePlatform platform = (GamePlatform)sender;

			AbstractMonster m = (AbstractMonster)platform.Monster;
			if(platform.Monster is Boss)
			{
				//is boss
				m.DoDamage(m.Health / 2);
				CurrencyTrayViewModel.OnClickAbilityProcess -= GerjanSmoothieAbility;
            }
		}
	}
}

