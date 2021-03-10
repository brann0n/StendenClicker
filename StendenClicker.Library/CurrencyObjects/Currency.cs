using StendenClicker.Library.AbstractMonster;
using System;

namespace StendenClicker.Library.CurrencyObjects
{
    public abstract class Currency
    {
        private Image image;

        public abstract double getValue(int multiplier);

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

    }

}

