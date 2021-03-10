namespace StendenClicker.Library.CurrencyObjects
{
	public class SparkCoin : Currency
	{
        public override double getValue(int multiplier)
        {
            return 1 * multiplier;
        }
    }

}

