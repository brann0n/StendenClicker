﻿using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StendenClickerGame
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class RegisterPage : Page
	{
		readonly ApiPlayerHandler beforeContextPlayerHandler;
		public RegisterPage()
		{
			this.InitializeComponent();

			beforeContextPlayerHandler = new ApiPlayerHandler();

			this.GoToMainPage.Click += GoToMainPage_Click;
			this.Loaded += RegisterPage_Loaded;
		}

		private async void RegisterPage_Loaded(object sender, RoutedEventArgs e)
		{
			//initialize all required resources.
			await Hero.Initialize();
			await Boss.Initialize();
			await Normal.Initialize();
			await NormalScene.Initialize();

			// check if this player has played before
			Player player = await beforeContextPlayerHandler.GetPlayerStateAsync(DeviceInfo.Instance.GetSystemId());
			if (!Player.IsPlayerObjectEmpty(player))
			{
				//load datacontext after resources have been initiated, otherwise you get errors.
				CreateDataContext();

				//continue with the app -> do not show signin page.
				this.Frame.Navigate(typeof(MainPage), this.DataContext);
			}
			else
			{
				//if there is no account available, show the loginbox.
				LoadingBox.Visibility = Visibility.Collapsed;
				LoginBox.Visibility = Visibility.Visible;
			}
		}

		private async void GoToMainPage_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(UsernameTextBox.Text))
			{
				if (await beforeContextPlayerHandler.IsUsernameAvailable(UsernameTextBox.Text))
				{
					try
					{
						await beforeContextPlayerHandler.CreateUser(UsernameTextBox.Text, DeviceInfo.Instance.GetSystemId());
						CreateDataContext();
						this.Frame.Navigate(typeof(MainPage), this.DataContext);
					}
					catch (Exception)
					{
						//show this error to the user.
						feedbackText.Text = "Er is een probleem opgetreden tijdens het aanmaken van uw account";
					}
				}
				else
				{
					feedbackText.Text = "Deze gebruikers naam is al in gebruik";  //give popup that username is already taken
				}
			}
			else
			{
				feedbackText.Text = "Er moet een gruikers naaw worden ingevuld";
			}
		}

		private void CreateDataContext()
		{
			this.DataContext = new MainPageViewModel();

			var bounds = Window.Current.Bounds;
			((MainPageViewModel)DataContext).WindowHeight = bounds.Height < 1080 ? 1080 : (int)bounds.Height;
			((MainPageViewModel)DataContext).WindowWidth = bounds.Width < 1920 ? 1920 : (int)bounds.Width;
		}
	}
}
