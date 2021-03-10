using StendenClickerGame.Factory;
using StendenClickerGame.PlayerControls;

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StendenClickerGame.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		private LevelGenerator levelGenerator;
		private ApiPlayerHandler playerContext;
		private Multiplayer.MultiplayerHubProxy mpProxy;

		public ICommand TestCommand { get; set; }

		public MainPageViewModel()
        {
			levelGenerator = new LevelGenerator();
			playerContext = new ApiPlayerHandler();
			mpProxy = new Multiplayer.MultiplayerHubProxy();


			TestCommand = new RelayCommand(() => 
			{
				mpProxy.SendTestToServer();
			});
        }


		/// <summary>
		/// The important notifier method of changed properties. This function should be called whenever you want to inform other classes that some property has changed.
		/// </summary>
		/// <param name="propertyName">The name of the updated property. Leaving this blank will fill in the name of the calling property.</param>
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public Player getPlayerContext()
        {
			return null;
        }

	}
	
}
