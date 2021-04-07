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
			AbilitiesList = new ObservableCollection<Abilities>()
			{

				new Abilities { AbilitieName = "Gerjan's Tarwesmoothie", AbilitieDescription = "Bezorgt een harde klap aan de volgende Boss! (Stackable)" , IsOffCooldown = false, Image = "Assets/koffie.png" , OnExecute = new RelayFunctionCommand<Abilities>(GerjanSmoothieAbilityClick, null) },
				new Abilities { AbilitieName = "Sji's Power Koffie", AbilitieDescription = "Dubbel de caffeïne, Dubbel de damage! (5s)", IsOffCooldown = true, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(SjiKoffieAbilityClickAsync) },
				new Abilities { AbilitieName = "Jan's Spa Bloedrood", AbilitieDescription = "Je vijand sparkelt uit elkaar!", IsOffCooldown = true, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(JanWaterAbilityClick)}

			};
		}

        private void JanWaterAbilityClick()
        {
			CurrencyTrayViewModel.OnClickAbilityProcess += JanWaterAbility;
        }

        private void JanWaterAbility(object sender, EventArgs e)
        {
			GamePlatform platform = (GamePlatform)sender;
			AbstractMonster m = (AbstractMonster)platform.Monster;
			m.DoDamage(m.Health);
			CurrencyTrayViewModel.OnClickAbilityProcess -= JanWaterAbility;			
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

        private async void GerjanSmoothieAbilityClick(Abilities test)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += GerjanSmoothieAbility;
			_AbilitiesList[0].IsOffCooldown = false;
			NotifyPropertyChanged("AbilitiesList");
			await Task.Delay(5000);
			_AbilitiesList[0].IsOffCooldown = true;
			NotifyPropertyChanged("AbilitiesList");
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

