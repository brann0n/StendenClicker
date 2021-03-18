using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Factory;
using StendenClicker.Library.PlayerControls;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace StendenClickerGame.ViewModels
{

	public class CurrencyTrayViewModel : ViewModelBase
	{
		private LevelGenerator levelGenerator;
		public List<Currency> DisplayableCurrency;

		public ICommand TappedEvent { get; set; }
		
		public CurrencyTrayViewModel()
		{
			TappedEvent = new RelayCommand(Test);

			levelGenerator = new LevelGenerator();
		}

		public void Test()
		{
			var level = levelGenerator.BuildLevel(new List<Player>() { new Player() });
			NotifyPropertyChanged("DisplayableCurrency");
		}
	}

}

