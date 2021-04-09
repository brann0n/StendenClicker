using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.CurrencyObjects
{
	public class ReusableCurrencyPool
	{
		private static readonly Lazy<ReusableCurrencyPool> instance = new Lazy<ReusableCurrencyPool>(() => new ReusableCurrencyPool());
		protected static ReusableCurrencyPool Instance { get { return instance.Value; } }
		private readonly List<Currency> Reusables;
		private int PoolSizeSC { get { return Reusables.Where(owo => owo is SparkCoin).Count(); } }
		private int PoolSizeEC { get { return Reusables.Where(uwu => uwu is EuropeanCredit).Count(); } }

		public const int PoolSizeSC_MAX = 50;
		public const int PoolSizeEC_MAX = 3;

		private ReusableCurrencyPool()
		{
			Reusables = new List<Currency>();
		}

		public static ReusableCurrencyPool GetInstance()
		{
			return instance.Value;
		}

		/// <summary>
		/// takes Currency from the Currency pool or creates new Currency objects, to return them for use.
		/// </summary>
		/// <param name="isSparkCoin">boolean to indicate the type of currency to acquire</param>
		/// <returns>Currency object</returns>

		public Currency AcquireReusable(bool isSparkCoin)
		{
			Currency currencyToReturn;
			if (isSparkCoin)
			{
				currencyToReturn = Reusables.FirstOrDefault(n => n is SparkCoin);
				if (currencyToReturn == null)
				{
					return CreateCurrency(true);
				}
			}
			else
			{
				currencyToReturn = Reusables.FirstOrDefault(n => n is EuropeanCredit);
				if (currencyToReturn == null)
				{
					return CreateCurrency(false);
				}
			}
			Reusables.Remove(currencyToReturn);
			return currencyToReturn;
		}

		/// <summary>
		/// creates a SparkCoin Object so it can be added to the ObjectPool
		/// </summary>
		/// <param name="isSparkCoin"></param>
		/// <returns></returns>
		private Currency CreateCurrency(bool isSparkCoin)
		{
			if (isSparkCoin)
			{
				return new SparkCoin();
			}
			else
			{
				return new EuropeanCredit();
			}
		}

		/// <summary>
		/// Releases Currency back to the list, or, if the pool is full, disposes of them.
		/// </summary>
		/// <param name="currency">Currency object to release</param>
		public void ReleaseCurrency(Currency currency)
		{
			if (currency is SparkCoin)
			{
				if (PoolSizeSC < PoolSizeSC_MAX)
				{
					Reusables.Add(currency);
					return;
				}

				currency.Dispose();
				return;
			}
			else if (currency is EuropeanCredit)
			{
				if (PoolSizeEC < PoolSizeEC_MAX)
				{
					Reusables.Add(currency);
					return;
				}

				currency.Dispose();
				return;
			}

			throw new Exception("Currency not registerd in the currency pool!");
		}

		public int GetPoolSizeEC() => PoolSizeEC;

		public int GetPoolSizeSC() => PoolSizeSC;
	}
}
