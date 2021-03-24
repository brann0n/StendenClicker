using StendenClicker.Library.Batches;
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

		private BatchedClick Clicks;

		private LevelGenerator levelGenerator;
		private ReusableCurrencyPool currencyPool;

		public CustomCoinList<Currency> CurrencyInView { get; set; }

		public ICommand TappedEvent { get; set; }
		public int MonsterHealthPercentage { get; set; } = 90;

		public CurrencyTrayViewModel()
		{
			TappedEvent = new RelayCommand(MonsterClicked);

			levelGenerator = new LevelGenerator();
			currencyPool = ReusableCurrencyPool.GetInstance();

			Clicks = new BatchedClick();

			CurrencyInView = new CustomCoinList<Currency>();
			CurrencyInView.OnCoinAdded += CurrencyInView_OnCoinAdded;
			CurrencyInView.OnCoinRemoved += CurrencyInView_OnCoinRemoved;
		}

		private void CurrencyInView_OnCoinRemoved(object sender, EventArgs e)
		{
			CurrencyRemoved?.Invoke(sender, e);
		}

		private void CurrencyInView_OnCoinAdded(object sender, System.EventArgs e)
		{
			CurrencyAdded?.Invoke(sender, e);
		}

		/// <summary>
		/// Most important function that handles all the game clicks
		/// </summary>
		public void MonsterClicked()
		{
			//todo: batch collect the clicks
			Clicks.addClick();
			CreateCoin(typeof(EuropeanCredit));
		}

		/// <summary>
		/// Handles coin creation through the CurrencyPool
		/// </summary>
		/// <param name="type">The type of coin to create</param>
		public void CreateCoin(Type type)
		{
			if (type.BaseType != typeof(Currency))
			{
				throw new Exception("No coin type was passed into the create coin function.");
			}

			Currency coin;
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
				//todo: add new value to wallet (possible lagswitch)

				//remove all hover events, this is needed to prevent double event firing after the coin is reused.
				((Currency)o).RemoveHoverEvents();

				//remove the coins from the view and list.
				CurrencyInView.Remove((Currency)o);
				currencyPool.ReleaseCurrency((Currency)o);
			};

			CurrencyInView.Add(coin);
		}

		public BatchedClick GetBatchedClick()
		{
			BatchedClick oldClicks = Clicks;
			Clicks = new BatchedClick();
			return oldClicks;
		}
	}

}

