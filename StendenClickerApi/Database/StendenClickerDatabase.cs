using Newtonsoft.Json;
using StendenClicker.Library.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace StendenClickerApi.Database
{
	public class StendenClickerDatabase : DbContext
	{
		public StendenClickerDatabase()
			: base("name=StendenClickerDatabase")
		{
		}

		// code first model documentation: http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<Monster> Monsters { get; set; }
		public virtual DbSet<Boss> Bosses { get; set; }
		public virtual DbSet<Hero> Heroes { get; set; }
		public virtual DbSet<ImageAsset> ImageAssets { get; set; }
		public virtual DbSet<Upgrade> Upgrades { get; set; }
		public virtual DbSet<Player> Players { get; set; }
		public virtual DbSet<PlayerHero> PlayerHeroes { get; set; }
		public virtual DbSet<Friendship> Friendships { get; set; }
		public virtual DbSet<MultiPlayerSession> Sessions { get; set; }
		public virtual DbSet<Scene> Scenes { get; set; }
	}

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
		public virtual ImageAsset BossAsset { get; set; }
	}

	public class Hero
	{
		public Hero()
		{
			Players = new HashSet<PlayerHero>();
		}
		public int HeroId { get; set; }
		public string HeroName { get; set; }
		public string HeroInformation { get; set; }
		public int HeroCost { get; set; }
		public virtual ImageAsset HeroAsset { get; set; }

		public virtual ICollection<Upgrade> Upgrades { get; set; }

		[JsonIgnore]
		public virtual ICollection<PlayerHero> Players { get; set; }
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

		[JsonIgnore]
		public virtual Hero Hero { get; set; }

		[JsonIgnore]
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

		public long __SparkCoins { get; set; }
		public long __EuropeanCredits { get; set; }

		[NotMapped]
		public ulong SparkCoins
		{
			get
			{
				unchecked
				{
					return (ulong)__SparkCoins;
				}
			}

			set
			{
				unchecked
				{
					__SparkCoins = (long)value;
				}
			}
		}

		[NotMapped]
		public ulong EuropeanCredits
		{
			get
			{
				unchecked
				{
					return (ulong)__EuropeanCredits;
				}
			}

			set
			{
				unchecked
				{
					__EuropeanCredits = (long)value;
				}
			}
		}

		public int MonstersDefeated { get; set; }
		public int BossesDefreated { get; set; }

		public virtual ICollection<PlayerHero> Heroes { get; set; }
		protected virtual ICollection<Friendship> Friendships1 { get; set; }
		protected virtual ICollection<Friendship> Friendships2 { get; set; }
		protected virtual ICollection<MultiPlayerSession> Sessions1 { get; set; }
		protected virtual ICollection<MultiPlayerSession> Sessions2 { get; set; }
		protected virtual ICollection<MultiPlayerSession> Sessions3 { get; set; }
		protected virtual ICollection<MultiPlayerSession> Sessions4 { get; set; }

		public static bool IsPlayerObjectEmpty(Player player)
		{
			if (player == null) return true;

			if (string.IsNullOrEmpty(player.PlayerGuid)) return true;

			if (string.IsNullOrEmpty(player.PlayerName)) return true;

			if (string.IsNullOrEmpty(player.DeviceId)) return true;

			return false;
		}

		public static implicit operator StendenClicker.Library.PlayerControls.Player(Player player)
		{
			return new StendenClicker.Library.PlayerControls.Player
			{
				deviceId = player.DeviceId,
				Username = player.PlayerName,
				UserId = Guid.Parse(player.PlayerGuid),
				State = new PlayerState { BossesDefeated = player.BossesDefreated, MonstersDefeated = player.MonstersDefeated },
				Wallet = new StendenClicker.Library.PlayerControls.PlayerCurrency { EuropeanCredit = player.EuropeanCredits, SparkCoin = player.SparkCoins }
			};
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

		[JsonIgnore]
		public virtual Player Player { get; set; }
		public virtual Hero Hero { get; set; }

		public virtual ICollection<Upgrade> Upgrades { get; set; }
	}

	public class Friendship
	{
		public int FriendshipId { get; set; }
		public virtual Player Player1 { get; set; }
		public virtual Player Player2 { get; set; }
	}

	public class MultiPlayerSession
	{
		//many-to-many-to-many-to-many? yes.
		public int MultiPlayerSessionId { get; set; }
		public virtual Player Player1 { get; set; }
		public virtual Player? Player2 { get; set; }
		public virtual Player? Player3 { get; set; }
		public virtual Player? Player4 { get; set; }
	}

	public class Scene
	{
		[Key]
		public int SceneId { get; set; }
		public string SceneName { get; set; }
		public virtual ImageAsset SceneAsset { get; set; }
	}
}