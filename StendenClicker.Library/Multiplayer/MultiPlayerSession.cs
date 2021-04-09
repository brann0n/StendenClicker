using StendenClicker.Library.Factory;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StendenClicker.Library.Multiplayer
{
	public class MultiPlayerSession : INotifyPropertyChanged
	{
		private List<Player> _currentPlayers;
		public List<Player> CurrentPlayerList { get { return _currentPlayers; } set { _currentPlayers = value; NotifyPropertyChanged(); } }
		private GamePlatform _currentLevel;
		public GamePlatform CurrentLevel { get { return _currentLevel; } set { _currentLevel = value; NotifyPropertyChanged(); } }
		public bool ForceUpdate { get; set; } = false;
		public string hostPlayerId { get; set; }
		public int maxPlayers { get; set; }
		public int monstersDefeated { get; set; }
		public int bossesDefeated { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}