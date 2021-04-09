using StendenClickerGame.ViewModels;
using System;
using Windows.ApplicationModel;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StendenClickerGame
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private readonly SystemMediaTransportControls systemControls;

		public MainPage()
		{
			this.InitializeComponent();

			systemControls = SystemMediaTransportControls.GetForCurrentView();

			// Register to handle the following system transport control buttons.
			systemControls.ButtonPressed += SystemControls_ButtonPressed;
			mediaElement.CurrentStateChanged += MediaElement_CurrentStateChanged;
			mediaElement.MediaEnded += MediaElement_MediaEnded;
			systemControls.IsPlayEnabled = true;
			systemControls.IsPauseEnabled = true;
			this.Loaded += MainPage_Loaded;
		}

		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			mediaElement.Position = TimeSpan.Zero;
			mediaElement.Play();
		}

		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			StartMusic();
		}

		private async void StartMusic()
		{
			var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
			if (folder != null)
			{
				var file = await folder.GetFileAsync("speaker-test-sample.wav");
				if (file != null)
				{
					var stream = await file.OpenReadAsync();
					mediaElement.SetSource(stream, file.ContentType);
					mediaElement.Volume = 0.15;
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			//this code-behind is needed to perform delayed loading of elements without stutter
			var ViewModelContext = e.Parameter as MainPageViewModel;
			this.DataContext = ViewModelContext;
		}

		private void ButtonFriend_OnClick(object sender, RoutedEventArgs e)
		{
			popup.Width = panel.ActualWidth;
			popup.Height = Window.Current.Bounds.Height;
			popup.IsOpen = true;
		}

		private void UitlegScreen_OnClick(object sender, RoutedEventArgs e)
		{
			popupUitleg.Width = panel.ActualWidth;
			popupUitleg.Height = Window.Current.Bounds.Height;
			popupUitleg.IsOpen = true;
		}

		private void CurrencyTray_OnClickAbilityProcess(object sender, EventArgs e)
		{
			MainPageViewModel vm = (MainPageViewModel)this.DataContext;
			vm.CurrencyTray.CurrentMonster.DamageFactor = 100;
		}

		private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
		{
			switch (mediaElement.CurrentState)
			{
				case MediaElementState.Playing:
					systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
					break;
				case MediaElementState.Paused:
					systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
					break;
				case MediaElementState.Stopped:
					systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
					break;
				case MediaElementState.Closed:
					systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;
					break;
				default:
					break;
			}
		}

		void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
		{
			switch (args.Button)
			{
				case SystemMediaTransportControlsButton.Play:
					PlayMedia();
					break;
				case SystemMediaTransportControlsButton.Pause:
					PauseMedia();
					break;
				case SystemMediaTransportControlsButton.Stop:
					StopMedia();
					break;
				default:
					break;
			}
		}

		private async void StopMedia()
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				mediaElement.Stop();
			});
		}

		async void PlayMedia()
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				if (mediaElement.CurrentState == MediaElementState.Playing)
					mediaElement.Pause();
				else
					mediaElement.Play();
			});
		}

		async void PauseMedia()
		{
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				mediaElement.Pause();
			});
		}
	}
}
