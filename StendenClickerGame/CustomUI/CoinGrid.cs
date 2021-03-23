using StendenClicker.Library.CurrencyObjects;
using StendenClickerGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StendenClickerGame.CustomUI
{
	public class CoinGrid : Grid
	{
		public static readonly DependencyProperty CurrenciesProperty =
			DependencyProperty.RegisterAttached("Currencies", typeof(CustomCoinList<Currency>), typeof(CoinGrid), new PropertyMetadata(default(CustomCoinList<Currency>), OnTypeChanged));

		public CustomCoinList<Currency> Currencies
		{
			get
			{
				//CheckEvents();
				return (CustomCoinList<Currency>)GetValue(CurrenciesProperty);
			}
			set
			{
				//CheckEvents();
				SetValue(CurrenciesProperty, value);
			}
		}

		private Dictionary<Currency, UIElement> Coins { get; set; }

		public CoinGrid()
		{
			Coins = new Dictionary<Currency, UIElement>();
			Currencies = new CustomCoinList<Currency>();
			CurrencyTrayViewModel.CurrencyAdded += Currencies_OnCoinAdded;
			CurrencyTrayViewModel.CurrencyRemoved += Currencies_OnCoinRemoved;
		}

		private void Currencies_OnCoinRemoved(object sender, EventArgs e)
		{
			//remove that button :)
			Remove((Currency)sender);
		}

		private void Currencies_OnCoinAdded(object sender, EventArgs e)
		{
			//add new button to layout :)
			Add((Currency)sender);
		}

		private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		public void Add(Currency coin)
		{
			Button NewCoinButton = new Button();
			NewCoinButton.Content = "Dikke test";
		
			Coins.Add(coin, NewCoinButton);
			Children.Add(NewCoinButton);


			var location = coin.dropCoordinates(new StendenClicker.Library.Point {X = 6, Y = 3 });
			Grid.SetColumn(NewCoinButton, location.X);
			Grid.SetRow(NewCoinButton, location.Y);
		}

		public void Remove(Currency currency)
		{
			Children.Remove(Coins[currency]);
			Coins.Remove(currency);
		}
	}
}
