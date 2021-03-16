using StendenClicker.Library.CurrencyObjects;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml.Input;

namespace StendenClickerGame.ViewModels
{

	public class CurrencyTrayViewModel : ViewModelBase
	{

		public List<Currency> DisplayableCurrency;

		public ICommand TappedEvent { get; set; }

		public CurrencyTrayViewModel()
		{
			TappedEvent = new RelayCommand(Test);
		}

		public void Test()
		{
			
		}
	}

}

