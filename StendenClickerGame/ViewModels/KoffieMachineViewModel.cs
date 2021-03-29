using StendenClicker.Library.Abilities;
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

		}
	}
}

