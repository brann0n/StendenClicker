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

		public new void Add(T item)
		{
			OnCoinAdded?.Invoke(item, null);

			base.Add(item);
		}

		public new void Remove(T item)
		{
			OnCoinRemoved?.Invoke(item, null);

			base.Remove(item);
		}
	}
}
