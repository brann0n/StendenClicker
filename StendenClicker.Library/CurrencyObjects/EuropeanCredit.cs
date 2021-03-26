using System;
using System.IO;

namespace StendenClicker.Library.CurrencyObjects
{
	public class EuropeanCredit : Currency
	{
        private static readonly Lazy<string> instance = new Lazy<string>(() => GetContent());
        public static string ImageContent { get { return instance.Value; } }

        public override ulong getValue(int multiplier)
        {
            return 1;
        }

        private static string GetContent()
        {
            return File.ReadAllText("Assets/ECcoin.xaml");
        }
    }

}

