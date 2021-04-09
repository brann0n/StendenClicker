using System;
using System.Threading;
using System.Threading.Tasks;

namespace StendenClicker.Library.CurrencyObjects
{
	public abstract class Currency : IDisposable
	{
		public event EventHandler OnCoinHover;
		public string CoinId { get; set; }
		public abstract ulong GetValue(int multiplier);

		private CancellationTokenSource CurrencyPickupCancellationSource;

		public Currency()
		{
			CoinId = Guid.NewGuid().ToString();
			CurrencyPickupCancellationSource = new CancellationTokenSource();
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

		protected virtual void Dispose(bool disposing)
		{
			//Dispose of other objects
			CurrencyPickupCancellationSource.Dispose();
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
			CurrencyPickupCancellationSource.Cancel();
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

		/// <summary>
		/// Sets the autoremove with specified timeframe.
		/// </summary>
		/// <param name="v"></param>
		public async void SetAutoRemove(int v)
		{
			try
			{
				await Task.Delay(v, CurrencyPickupCancellationSource.Token);
				OnCoinHover?.Invoke(this, null);
			}
			catch (TaskCanceledException)
			{
				CurrencyPickupCancellationSource = new CancellationTokenSource();
			}
			catch (Exception)
			{
				//ok, dikke error
				throw; // smijt weg dat ding
			}
		}

		/// <summary>
		/// Sets the autoremove with a randomized timer
		/// </summary>
		public async void SetAutoRemove()
		{
			Random r = new Random();
			SetAutoRemove(r.Next(4000, 6000));
		}
	}
}