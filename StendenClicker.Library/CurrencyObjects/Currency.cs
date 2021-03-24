using StendenClicker.Library.AbstractMonster;
using System;

namespace StendenClicker.Library.CurrencyObjects
{
    public abstract class Currency : IDisposable
    {
        private Image image;
        public event EventHandler OnCoinHover;
        public string CoinId { get; set; }
        public abstract double getValue(int multiplier);
        public Currency()
		{
            CoinId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Function that decides where to place a coin inside a specified grid size
        /// </summary>
        /// <param name="windowSize">the x,y size of the grid you want to place this object inside of.</param>
        /// <returns>Random coordinates that lie inside the specified size</returns>
        public Point dropCoordinates(Point windowSize)
        {
            Random r = new Random();
            return new Point { X = r.Next(0, windowSize.X), Y = r.Next(0, windowSize.Y) };
        }

        public Image getSprite()
        {
            return image;
        }
        protected virtual void Dispose(bool disposing)
        {
            image = null;
        }

        public void Dispose()
		{           
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public void Hovered()
		{
            OnCoinHover?.Invoke(this, null);
		}

		public override string ToString()
		{
			return CoinId.ToString();
		}

        public void RemoveHoverEvents()
        {
            foreach (Delegate d in OnCoinHover.GetInvocationList())
            {
                OnCoinHover -= (EventHandler)d;
            }
        }
    }

}

