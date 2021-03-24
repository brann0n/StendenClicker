using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Factory;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.CustomUI;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace StendenClickerGame.ViewModels
{

	public class CurrencyTrayViewModel : ViewModelBase
	{
		public static event EventHandler CurrencyAdded;
		public static event EventHandler CurrencyRemoved;

		private LevelGenerator levelGenerator;
		public List<Currency> DisplayableCurrency;

		public CustomCoinList<Currency> TestCoins { get; set; }

		public ICommand TappedEvent { get; set; }
		
		public CurrencyTrayViewModel()
		{
			TappedEvent = new RelayCommand(Test);

			levelGenerator = new LevelGenerator();

			TestCoins = new CustomCoinList<Currency>();
			TestCoins.OnCoinAdded += TestCoins_OnCoinAdded;
			TestCoins.OnCoinRemoved += TestCoins_OnCoinRemoved;
		}

		private void TestCoins_OnCoinRemoved(object sender, EventArgs e)
		{
			CurrencyRemoved?.Invoke(sender, e);
		}

		private void TestCoins_OnCoinAdded(object sender, System.EventArgs e)
		{
			CurrencyAdded?.Invoke(sender, e);
		}

		public void Test()
		{
			var coin = new SparkCoin() { };

			coin.OnCoinHover += (o, e) => 
			{
				TestCoins.Remove((Currency)o);
			};

			TestCoins.Add(coin);
		}

		
	}

}

