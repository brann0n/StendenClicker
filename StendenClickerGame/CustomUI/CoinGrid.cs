using StendenClicker.Library.CurrencyObjects;
using StendenClickerGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

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
			Button NewCoinButton = new Button {
				Background = new SolidColorBrush(Colors.Transparent),
				Margin = new Thickness(0),
				Padding = new Thickness(0)
			};

			NewCoinButton.PointerEntered += (o,e) => 
			{
				coin.Hovered();
			};
			
			Viewbox coinViewbox = new Viewbox {
				Visibility = Visibility
			};

			Canvas coinCanvas = new Canvas();

			if (coin is SparkCoin)
			{
				coinCanvas.Height = 100;
				coinCanvas.Width = 100;

				Path pathRedCircle = new Path();
				Path pathSpark = new Path();

				var dataRedCircle = "M50 100C22.35 100 0 77.65 0 50C0 22.35 22.35 0 50 0C77.65 0 100 22.35 100 50C100 77.65 77.65 100 50 100Z";
				var dataSpark = "M30 77L72 46L54 36L60 18L32 47L49 47L30 77Z";

				var geometryRedCircle = (Geometry)XamlReader.Load(
				"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
				+ dataRedCircle + "</Geometry>");

				var geometrySpark = (Geometry)XamlReader.Load(
					"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
					+ dataSpark + "</Geometry>");

				pathRedCircle.Data = geometryRedCircle;
				pathSpark.Data = geometrySpark;

				pathRedCircle.Fill = new SolidColorBrush(Colors.Red);
				pathSpark.Fill = new SolidColorBrush(Colors.White);

				coinCanvas.Children.Add(pathRedCircle);
				coinCanvas.Children.Add(pathSpark);
			}
			else if (coin is EuropeanCredit)
			{
				coinCanvas.Height = 618;
				coinCanvas.Width = 618;

				
			}
            else{ };

			coinViewbox.Child = coinCanvas;
			NewCoinButton.Content = coinViewbox;
			Coins.Add(coin, NewCoinButton);
			Children.Add(NewCoinButton);

			var location = coin.dropCoordinates(new StendenClicker.Library.Point {X = 15, Y = 3 });
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
