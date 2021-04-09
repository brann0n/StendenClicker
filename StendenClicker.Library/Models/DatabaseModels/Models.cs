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

		public virtual ICollection<Upgrade> Upgrades { get; set; }

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
		public Upgrade()
		{
			PlayerHeroes = new HashSet<PlayerHero>();
		}

		public int UpgradeId { get; set; }
		public string UpgradeName { get; set; }
		public int UpgradeCost { get; set; }
		public bool UpgradeIsAbility { get; set; }
		public virtual Hero Hero { get; set; }

		public virtual ICollection<PlayerHero> PlayerHeroes { get; set; }
	}

	public class Player
	{
		public Player()
		{
			Heroes = new HashSet<PlayerHero>();
		}
		public int PlayerId { get; set; }
		public string PlayerGuid { get; set; }
		public string PlayerName { get; set; }
		public string DeviceId { get; set; }

		public ulong SparkCoins { get; set; }
		public ulong EuropeanCredits { get; set; }

		public int MonstersDefeated { get; set; }
		public int BossesDefreated { get; set; }

		public virtual ICollection<PlayerHero> Heroes { get; set; }

		public static implicit operator Player(PlayerControls.Player player)
		{
			return new Player
			{
				DeviceId = player.deviceId,
				PlayerGuid = player.UserId.ToString(),
				PlayerName = player.Username,
				BossesDefreated = player.State.BossesDefeated,
				MonstersDefeated = player.State.MonstersDefeated,
				SparkCoins = player.Wallet.SparkCoin,
				EuropeanCredits = player.Wallet.EuropeanCredit,
				Heroes = player.Heroes
			};
		}

		public static bool IsPlayerObjectEmpty(Player player)
		{
			if (player == null) return true;

			if (string.IsNullOrEmpty(player.PlayerGuid)) return true;

			if (string.IsNullOrEmpty(player.PlayerName)) return true;
			
			if (string.IsNullOrEmpty(player.DeviceId)) return true;

			return false;
		}
	}

	public class PlayerHero
	{
		public PlayerHero()
		{
			Upgrades = new HashSet<Upgrade>();
		}
		public int PlayerHeroId { get; set; }
		public int HeroUpgradeLevel { get; set; }
		public int SpecialUpgradeLevel { get; set; }

		public virtual Player Player { get; set; }
		public virtual Hero Hero { get; set; }

		public virtual ICollection<Upgrade> Upgrades { get; set; }


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
