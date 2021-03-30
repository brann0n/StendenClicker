using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StendenClickerGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        ApiPlayerHandler beforeContextPlayerHandler;
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
                this.DataContext = new MainPageViewModel();

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
                //todo: check if the username has already been taken (although it doenst require to be unique, for searching friends it might be usefull)
               // MainPageViewModel context = (MainPageViewModel)this.DataContext;
                try
                {
                    await beforeContextPlayerHandler.CreateUser(UsernameTextBox.Text, DeviceInfo.Instance.GetSystemId());
                    this.DataContext = new MainPageViewModel();
                    this.Frame.Navigate(typeof(MainPage), this.DataContext);
                }
                catch (Exception)
                {
                    //show this error to the user.
                }              
            }

            //show a red label with the message that they need to enter a username.
        }
    }
}
