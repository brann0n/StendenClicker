
using System;
using System.Collections.Generic;

namespace StendenClicker.Library.CurrencyObjects
{
    public class ReusableCurrencyPool
    {
        private static readonly Lazy<ReusableCurrencyPool> instance = new Lazy<ReusableCurrencyPool>(() => new ReusableCurrencyPool());
        protected static ReusableCurrencyPool Instance { get { return instance.Value; } }

        private List<Currency> reusables;
        private int PoolSizeSC;
        private int PoolSizeEC;


        private ReusableCurrencyPool()
        {
            PoolSizeSC = 0;
            PoolSizeEC = 0;

            reusables = new List<Currency>();
        }

        public static ReusableCurrencyPool getInstance()
        {
            return instance.Value;
        }

        public Currency aquireReusable()
        {
            return null;
        }

        public Currency releaseReusable(int index)
        {
            return null;
        }

        public void setMaxPoolSize(int scSize, int ecSize)
        {

        }

        public void visualPoolSize(int size)
        {

        }

    }

}

