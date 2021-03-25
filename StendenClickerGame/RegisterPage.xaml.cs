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
        public RegisterPage()
        {
            this.InitializeComponent();
            this.DataContext = new MainPageViewModel();
            this.GoToMainPage.Click += GoToMainPage_Click;
			this.Loaded += RegisterPage_Loaded;
        }

		private async void RegisterPage_Loaded(object sender, RoutedEventArgs e)
		{
            await Hero.Initialize();
            await Boss.Initialize();
            await Normal.Initialize();
            await NormalScene.Initialize();
            await BossScene.Initialize();
		}

		private void GoToMainPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), this.DataContext);
        }
    }
}
