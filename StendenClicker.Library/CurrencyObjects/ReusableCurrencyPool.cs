
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StendenClicker.Library.CurrencyObjects
{
    public class ReusableCurrencyPool
    {
        private static readonly Lazy<ReusableCurrencyPool> instance = new Lazy<ReusableCurrencyPool>(() => new ReusableCurrencyPool());
        protected static ReusableCurrencyPool Instance { get { return instance.Value; } }
        
        private List<Currency> reusables;
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
        public static ReusableCurrencyPool getInstance()
        {
            return instance.Value;
        }
        
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

        private int PoolSizeSC;
        private int PoolSizeEC;
        private static int PoolSizeSC_MAX = 5;
        private static int PoolSizeEC_MAX = 5;
        private ReusableCurrencyPool()
        {
            PoolSizeSC = 0;
            PoolSizeEC = 0;
            reusables = new List<Currency>();
        }

        // returns a SparkCoin object so it can be dropped on the platform after defeating
        // a monster
        // TODO convert to Currency based function.
        public Currency acquireReusable(Boolean isSparkCoin)
        {
            Currency currencyToReturn = null;
            if (isSparkCoin)
            {
                if (PoolSizeSC > 0)
                {
                    if (reusables.Count > 0)
                    {
                        foreach(Currency currency in reusables)
                        {
                            if(currency.GetType().ToString() == "SparkCoin")
                            {
                                currencyToReturn = currency;
                                reusables.Remove(currency);
                                return currencyToReturn;
                            }
                        }
                        //if it gets here, there are no Sparkcoins in the list and thus needs a force released sparkcoin
                    }
                    else
                    {
                        if (PoolSizeSC < PoolSizeSC_MAX)
                        {
                            currencyToReturn = createCurrency(true);
                            return currencyToReturn;
                        }
                        else
                        {
                            //TODO Force Release a coin so it can be dropped with a new value
                        }
                    }
                }
                else
                {
                    currencyToReturn = createCurrency(true);
                    return currencyToReturn;
                }
            } 
            else
            {
                if(PoolSizeEC > 0)
                {
                    if (reusables.Count > 0)
                    {
                        foreach (Currency currency in reusables)
                        {
                            if (currency.GetType().ToString() == "EuropeanCredit")
                            {
                                currencyToReturn = currency;
                                reusables.Remove(currency);
                                return currencyToReturn;
                            }                           
                        }
                        //if it gets here, there are no EuropeanCredits in the list and thus needs a force released EC
                    }
                    else
                    {
                        if (PoolSizeEC < PoolSizeEC_MAX)
                        {
                            currencyToReturn = createCurrency(false);
                            return currencyToReturn;
                        }
                        else
                        {
                            //TODO Force Release a coin so it can be dropped with a new value
                        }
                    }
                }
                else
                {
                    currencyToReturn = createCurrency(false);
                    return currencyToReturn;
                }
            }
            return currencyToReturn;
        }

        // creates a SparkCoin Object so it can be added to the ObjectPool
        private Currency createCurrency(Boolean isSparkCoin)
        {
            if(isSparkCoin)
            {
                SparkCoin newSparkcoin = new SparkCoin();
                PoolSizeSC++;
                return newSparkcoin;
            } else
            {
                EuropeanCredit newEuropeanCredit = new EuropeanCredit();
                PoolSizeEC++;
                return newEuropeanCredit;
            }
        } 

        // adds the picked up spark coin back to the bag
        public void ReleaseCurrency(Currency currency)
        {
            reusables.Add(currency);
        }
    }
}