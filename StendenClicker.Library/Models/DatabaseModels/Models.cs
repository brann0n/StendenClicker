using System;
using System.Collections.Generic;
using System.Text;

namespace StendenClicker.Library.Models.DatabaseModels
{
	public class Monster
	{
		public int MonsterId { get; set; }
		public string MonsterName { get; set; }
		public int BaseHealth { get; set; }
		public virtual ImageAsset MonsterAsset { get; set; }
	}

	public class Boss
	{
		public int BossId { get; set; }
		public string BossName { get; set; }
		public int BaseHealth { get; set; }
		public ImageAsset BossAsset { get; set; }
	}

	public class Hero
	{
		public Hero()
		{

		}
		public int HeroId { get; set; }
		public string HeroName { get; set; }
		public string HeroInformation { get; set; }
		public int HeroCost { get; set; }
		public virtual ImageAsset HeroAsset { get; set; }

		protected virtual ICollection<Upgrade> Upgrades { get; set; }

	}

	public class ImageAsset
	{
		public int ImageAssetId { get; set; }
		public string ImageDescription { get; set; }
		public string Base64Image { get; set; }

		protected virtual ICollection<Monster> Monsters { get; set; }
		protected virtual ICollection<Boss> Bosses { get; set; }
		protected virtual ICollection<Hero> Heroes { get; set; }
		protected virtual ICollection<Scene> Scenes { get; set; }
	}

	public class Upgrade
	{
		public int UpgradeId { get; set; }
		public string UpgradeName { get; set; }
		public int UpgradeCost { get; set; }
		public bool UpgradeIsAbility { get; set; }
		public virtual Hero Hero { get; set; }
	}

	public class Player
	{
		public Player()
		{
			Heroes = new HashSet<Hero>();
		}
		public int PlayerId { get; set; }
		public string PlayerGuid { get; set; }
		public string PlayerName { get; set; }
		public string DeviceId { get; set; }
		public string ConnectionId { get; set; }

		public virtual ICollection<Hero> Heroes { get; set; }

		public static implicit operator Player(PlayerControls.Player player)
		{
			return new Player
			{
				ConnectionId = player.connectionId,
				DeviceId = player.deviceId,
				PlayerGuid = player.UserId.ToString(),
				PlayerName = player.Username
			};
		}
	}

	public class Friendship
	{
		public int FriendshipId { get; set; }
		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
	}

	public class MultiPlayerSession
	{
		//many-to-many-to-many-to-many?
		public int MultiPlayerSessionId { get; set; }
		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
		public Player Player3 { get; set; }
		public Player Player4 { get; set; }
	}

	public class Scene
	{
		public int SceneId { get; set; }
		public string SceneName { get; set; }
		public ImageAsset SceneAsset { get; set; }
	}
}
