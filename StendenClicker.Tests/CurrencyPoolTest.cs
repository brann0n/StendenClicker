using Microsoft.VisualStudio.TestTools.UnitTesting;
using StendenClicker.Library.CurrencyObjects;
using System;
using System.Collections.Generic;

namespace StendenClicker.Tests
{
	[TestClass]
	public class CurrencyPoolTest
	{
		private ReusableCurrencyPool pool;

		[TestInitialize]
		public void Prepare()
		{
			pool = ReusableCurrencyPool.GetInstance();
		}


		[TestMethod]
		public void TestRelease()
		{
			List<Currency> otherPool = new List<Currency>();

			for (int i = 0; i < 12; i++)
			{
				if (i % 2 == 0)
					otherPool.Add(pool.AcquireReusable(true));
				else
					otherPool.Add(pool.AcquireReusable(false));
			}

			foreach(Currency c in otherPool)
			{
				pool.ReleaseCurrency(c);
			}

			Assert.AreEqual(pool.GetPoolSizeEC(), ReusableCurrencyPool.PoolSizeEC_MAX);
			Assert.AreEqual(pool.GetPoolSizeSC(), ReusableCurrencyPool.PoolSizeSC_MAX);
		}

		[TestMethod]
		public void TestAcquire()
		{
			Currency sparkcoin = pool.AcquireReusable(true);
			Currency europeancredit = pool.AcquireReusable(false);
			Assert.AreEqual(sparkcoin is SparkCoin, true);
			Assert.AreEqual(europeancredit is EuropeanCredit, true);
		}
	}
}
