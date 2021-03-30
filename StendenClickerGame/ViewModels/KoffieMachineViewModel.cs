using StendenClicker.Library.Abilities;
using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.Factory;
using StendenClickerGame.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
				new abilities { AbilitieName = "Koffie", Cooldown = 100, Image = "Assets/koffie.png" , OnExecute = new RelayCommand(KoffieAbilityClick) },
				new abilities { AbilitieName = "Water", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" }
			};
		}

		private void KoffieAbilityClick()
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += CurrencyTrayViewModel_OnClickAbilityProcess;

		}

		private void CurrencyTrayViewModel_OnClickAbilityProcess(object sender, System.EventArgs e)
		{
			GamePlatform platform = (GamePlatform)sender;

			AbstractMonster m = (AbstractMonster)platform.Monster;

			if(platform.Monster is Boss)
			{
				//is boss
			}
		}
	}
}

