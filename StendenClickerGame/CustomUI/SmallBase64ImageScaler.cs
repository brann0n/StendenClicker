using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace StendenClickerGame.CustomUI
{
	public class SmallBase64ImageScaler : IValueConverter
	{
		public async Task<BitmapImage> Convert(object value)
		{
			byte[] bytes = System.Convert.FromBase64String((string)value);
			BitmapImage image = null;
			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			try
			{
				await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
				{
					image = new BitmapImage();
					InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
					await stream.WriteAsync(bytes.AsBuffer());
					stream.Seek(0);

					await image.SetSourceAsync(stream);

					image.DecodePixelHeight = 100;
					image.DecodePixelWidth = 100;
				});
			}
			catch (Exception)
			{

				throw;
			}
			return image;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return null;
		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var task = Task.Run(() => Convert((string)value));
			return new TaskCompletionNotifier<BitmapImage>(task);
		}
	}
}
