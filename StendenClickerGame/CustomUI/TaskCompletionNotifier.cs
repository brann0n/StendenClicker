using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace StendenClickerGame.CustomUI
{
	public sealed class TaskCompletionNotifier<TResult> : INotifyPropertyChanged
	{
		public TaskCompletionNotifier(Task<TResult> task)
		{
			Task = task;
			if (!task.IsCompleted)
			{
				var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
				task.ContinueWith(t =>
				{
					var propertyChanged = PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
						if (t.IsCanceled)
						{
							propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
						}
						else if (t.IsFaulted)
						{
							propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
							propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
						}
						else
						{
							propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
							propertyChanged(this, new PropertyChangedEventArgs("Result"));
						}
					}
				},
				CancellationToken.None,
				TaskContinuationOptions.ExecuteSynchronously,
				scheduler);
			}
		}

		public Task<TResult> Task { get; private set; }

		public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default; } }

		public bool IsCompleted { get { return Task.IsCompleted; } }

		public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

		public bool IsCanceled { get { return Task.IsCanceled; } }

		public bool IsFaulted { get { return Task.IsFaulted; } }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
