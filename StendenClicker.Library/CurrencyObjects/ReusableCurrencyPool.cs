
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StendenClicker.Library.CurrencyObjects
{
    public class ReusableCurrencyPool
    {
        //private static readonly Lazy<ReusableCurrencyPool> instance = new Lazy<ReusableCurrencyPool>(() => new ReusableCurrencyPool());
        //protected static ReusableCurrencyPool Instance { get { return instance.Value; } }
        //
        //private List<Currency> reusables;
        //private int PoolSizeSC;
        //private int PoolSizeEC;
        //private static int PoolSizeSC_MAX = 5;
        //private static int PoolSizeEC_MAX = 5;
        //
        //
        //private ReusableCurrencyPool()
        //{
        //    PoolSizeSC = 0;
        //    PoolSizeEC = 0;
        //
        //    reusables = new List<Currency>();
        //}
        //
        //public static ReusableCurrencyPool getInstance()
        //{
        //    return instance.Value;
        //}
        //
        //public Currency aquireReusable()
        //{
        //    return null;
        //}
        //
        //public Currency releaseReusable(int index)
        //{
        //    return null;
        //}
        //
        //public void setMaxPoolSize(int scSize, int ecSize)
        //{
        //
        //}
        //
        //public void visualPoolSize(int size)
        //{
        //
        //}

        private ConcurrentBag<SparkCoin> _sparkCoinBag;
        private ConcurrentBag<EuropeanCredit> _europeanCreditsBag;
        private int PoolSizeSC;
        private int PoolSizeEC;
        private static int MaxSparkCoinCount = 10;
        private static int MaxEuropeanCreditCount = 5;

        public ReusableCurrencyPool()
        {
            PoolSizeSC = 0;
            PoolSizeEC = 0;
            _sparkCoinBag = new ConcurrentBag<SparkCoin>();
            _europeanCreditsBag = new ConcurrentBag<EuropeanCredit>();
        }

        // returns a SparkCoin object so it can be dropped on the platform after defeating
        // a monster
        // TODO convert to Currency based function.
        public SparkCoin GetSparkCoin()
        {
            SparkCoin sparkcoin;
            if(PoolSizeSC > 0)
            {
                if (_sparkCoinBag.TryTake(out sparkcoin))
                {
                    return sparkcoin;
                }
                else
                {
                    if (PoolSizeSC < MaxSparkCoinCount)
                    {
                        sparkcoin = createSparkCoin();
                    } 
                    else
                    {
                        //TODO Force Release a coin so it can be dropped with a new value
                    }
                }
            }
            else
            {
                sparkcoin = createSparkCoin();
            }
            return sparkcoin;
        }

        // creates a SparkCoin Object so it can be added to the ObjectPool
        private SparkCoin createSparkCoin()
        {
            SparkCoin newSparkcoin = new SparkCoin();
            PoolSizeSC++;
            return newSparkcoin;
        } 

        // adds the picked up spark coin back to the bag
        public void ReleaseSparkCoin(SparkCoin sparkcoin)
        {
            _sparkCoinBag.Add(sparkcoin);
        }
    }
}

