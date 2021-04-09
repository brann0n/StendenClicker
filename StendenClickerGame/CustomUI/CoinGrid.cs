using StendenClicker.Library.CurrencyObjects;
using StendenClickerGame.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace StendenClickerGame.CustomUI
{
	public class CoinGrid : Grid
	{
		public readonly DependencyProperty CurrenciesProperty =
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
			Button NewCoinButton = new Button
			{
				Background = new SolidColorBrush(Colors.Transparent),
				Margin = new Thickness(0),
				Padding = new Thickness(0)
			};

			NewCoinButton.PointerEntered += (o, e) =>
			{
				coin.Hovered();
			};

			Viewbox coinViewbox = null;

			//loading this lazy into memory is not possible with XAML root addresses.
			if (coin is SparkCoin)
			{
				coinViewbox = (Viewbox)XamlReader.Load(SparkCoin.ImageContent);
			}
			else if (coin is EuropeanCredit)
			{
				coinViewbox = (Viewbox)XamlReader.Load(EuropeanCredit.ImageContent);
			}

			//coinViewbox.Child = coinCanvas;
			NewCoinButton.Content = coinViewbox;
			Coins.Add(coin, NewCoinButton);
			Children.Add(NewCoinButton);

			var location = coin.DropCoordinates(new StendenClicker.Library.Point { X = 15, Y = 3 });
			Grid.SetColumn(NewCoinButton, location.X);
			Grid.SetRow(NewCoinButton, location.Y);
		}

		public void Remove(Currency currency)
		{
			Children.Remove(Coins[currency]);
			Coins.Remove(currency);
			if (Coins.Count == 0)
			{
				MainPageViewModel.DoSave();
			}
		}
	}
}
