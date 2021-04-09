using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Factory;
using StendenClickerGame.Data;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace StendenClickerGame.ViewModels
{
	public class KoffieMachineViewModel : ViewModelBase
	{
		private static ObservableCollection<Abilities> StaticAbilitiesList { get; set; }
		public ObservableCollection<Abilities> AbilitiesList { get => StaticAbilitiesList; set => StaticAbilitiesList = value; }

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
			SelfContext.ContextSetAbilityEnabled();

			CurrencyTrayViewModel.OnClickAbilityProcess += MartijnSportAbility;

			await SelfContext.ContextDelayProgressbarEmpty(15000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= MartijnSportAbility;
			await SelfContext.ContextDelayProgressbarFill(285000);

			SelfContext.ContextSetAbilityDisabled();
		}

		private void MartijnSportAbility(GamePlatform sender, BatchedClick e)
		{
			CurrencyTrayViewModel.AbilityMultiplier += 4;
		}

		private async void MiguelTheeAbilityClick(Abilities SelfContext)
		{
			SelfContext.ContextSetAbilityEnabled();

			CurrencyTrayViewModel.OnClickAbilityProcess += MiguelTheeAbility;

			await SelfContext.ContextDelayProgressbarEmpty(10000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= MiguelTheeAbility;
			await SelfContext.ContextDelayProgressbarFill(230000);

			SelfContext.ContextSetAbilityDisabled();
		}

		private void MiguelTheeAbility(GamePlatform sender, BatchedClick e)
		{
			CurrencyTrayViewModel.AbilityMultiplier += 2;
		}

		private async void JanWaterAbilityClick(Abilities SelfContext)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += JanWaterAbility;

			SelfContext.ContextSetAbilityEnabled();

			await SelfContext.ContextDelayProgressbarFill(300000);

			SelfContext.ContextSetAbilityDisabled();
		}

		private void JanWaterAbility(GamePlatform sender, BatchedClick e)
		{
			GamePlatform platform = (GamePlatform)sender;
			AbstractMonster m = (AbstractMonster)platform.Monster;
			int Damage = m.GetHealth();
			m.DoDamage(Damage);
			e.AddDamage(Damage);
			CurrencyTrayViewModel.OnClickAbilityProcess -= JanWaterAbility;
		}

		private async void SjiKoffieAbilityClick(Abilities SelfContext)
		{
			SelfContext.ContextSetAbilityEnabled();

			CurrencyTrayViewModel.OnClickAbilityProcess += SjiKoffieAbility;

			await SelfContext.ContextDelayProgressbarEmpty(5000);
			CurrencyTrayViewModel.OnClickAbilityProcess -= SjiKoffieAbility;
			await SelfContext.ContextDelayProgressbarFill(115000);

			SelfContext.ContextSetAbilityDisabled();
		}

		private void SjiKoffieAbility(GamePlatform sender, BatchedClick e)
		{
			CurrencyTrayViewModel.AbilityMultiplier += 1;
		}

		private async void GerjanSmoothieAbilityClick(Abilities SelfContext)
		{
			CurrencyTrayViewModel.OnClickAbilityProcess += GerjanSmoothieAbility;

			SelfContext.ContextSetAbilityEnabled();

			await SelfContext.ContextDelayProgressbarFill(150000);

			SelfContext.ContextSetAbilityDisabled();
		}

		private void GerjanSmoothieAbility(GamePlatform sender, BatchedClick e)
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

