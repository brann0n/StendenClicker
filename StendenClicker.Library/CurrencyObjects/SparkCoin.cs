using System;
using System.IO;

namespace StendenClicker.Library.CurrencyObjects
{
	public class SparkCoin : Currency
	{

        private static readonly Lazy<string> instance = new Lazy<string>(() => GetContent());
        public static string ImageContent { get { return instance.Value; } }

        public override double getValue(int multiplier)
        {
            return 1 * multiplier;
        }

        private static string GetContent()
		{
            return File.ReadAllText("Assets/Sparkcoin.xaml");
		}
    }

}

