using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StendenClickerGame.CustomUI
{
	public class CustomCoinList<T>: List<T>
	{
		public event EventHandler OnCoinAdded;
		public event EventHandler OnCoinRemoved;

		/// <summary>
		/// Adds the object to the list, and if assigned, fires the object added event.
		/// </summary>
		/// <param name="item"></param>
		public new void Add(T item)
		{
			OnCoinAdded?.Invoke(item, null);

			base.Add(item);
		}

		/// <summary>
		/// Removes the object from the list, and if assigned, fires the object removed event.
		/// </summary>
		/// <param name="item"></param>
		public new void Remove(T item)
		{
			OnCoinRemoved?.Invoke(item, null);

			base.Remove(item);
		}
	}
}
