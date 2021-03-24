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
		private ReusableCurrencyPool currencyPool;

		public CustomCoinList<Currency> CurrencyInView { get; set; }

		public ICommand TappedEvent { get; set; }

		public CurrencyTrayViewModel()
		{
			TappedEvent = new RelayCommand(MonsterClicked);

			levelGenerator = new LevelGenerator();
			currencyPool = ReusableCurrencyPool.GetInstance();

			CurrencyInView = new CustomCoinList<Currency>();
			CurrencyInView.OnCoinAdded += TestCoins_OnCoinAdded;
			CurrencyInView.OnCoinRemoved += TestCoins_OnCoinRemoved;
		}

		private void TestCoins_OnCoinRemoved(object sender, EventArgs e)
		{
			CurrencyRemoved?.Invoke(sender, e);
		}

		private void TestCoins_OnCoinAdded(object sender, System.EventArgs e)
		{
			CurrencyAdded?.Invoke(sender, e);
		}

		/// <summary>
		/// Most important function that handles all the game clicks
		/// </summary>
		public void MonsterClicked()
		{


			CreateCoin(typeof(SparkCoin));
		}

		public void CreateCoin(Type type)
		{
			if (type.BaseType != typeof(Currency))
			{
				throw new Exception("No coin type was passed into the create coin function.");
			}

			Currency coin;

			//TODO: these instance creating should happen in the object pool and not here
			if (type == typeof(SparkCoin))
			{
				coin = currencyPool.AcquireReusable(true);
			}
			else if (type == typeof(EuropeanCredit))
			{
				coin = currencyPool.AcquireReusable(false);
			}
			else
			{
				throw new Exception("Unkown coin type was passed into this create coin function.");
			}

			coin.OnCoinHover += (o, e) =>
			{
				CurrencyInView.Remove((Currency)o);
				((Currency)o).RemoveHoverEvents();
				currencyPool.ReleaseCurrency((Currency)o);
			};

			CurrencyInView.Add(coin);
		}
	}

}

