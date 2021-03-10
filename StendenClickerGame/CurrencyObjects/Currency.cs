using StendenClickerGame.AbstractMonster;
using Windows.UI.Xaml.Controls;

namespace StendenClickerGame.CurrencyObjects
{
	public abstract class Currency
	{
		private Image image;

		public abstract double getValue(int multiplier);

		public Point dropCoordinates(Point windowSize)
		{
			return new Point { };
		}

	}

}

