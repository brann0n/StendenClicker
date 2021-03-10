using StendenClickerGame.AbstractPlatform;
using StendenClickerGame.PlayerControls;
using System.Collections.Generic;

namespace StendenClickerGame.Multiplayer
{
	public class MultiPlayerSession
	{
		public List<Player> currentPlayerList { get; set; }
		public IAbstractPlatform currentLevel { get; set; }
		public string hostPlayerId { get; set; }
		public int maxPlayers { get; set; }
		public int monstersDefeated { get; set; }
		public int bossesDefeated { get; set; }
	}
}

