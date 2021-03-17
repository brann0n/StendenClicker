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
		// Your context has been configured to use a 'StendenClickerDatabase' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'StendenClickerApi.Database.StendenClickerDatabase' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'StendenClickerDatabase' 
		// connection string in the application configuration file.
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
		[Key]
		public int MonsterId { get; set; }
		public string MonsterName { get; set; }
		public int BaseHealth { get; set; }
		public int MonsterAssetRefId { get; set; }
		public ImageAsset MonsterAsset { get; set; }
	}

	public class Boss
	{
		[Key]
		public int BossId { get; set; }
		public string BossName { get; set; }
		public int BaseHealth { get; set; }
		public int BossAssetRefId { get; set; }
		public ImageAsset BossAsset { get; set; }
	}

	public class Hero
	{
		[Key]
		public int HeroId { get; set; }
		public string HeroName { get; set; }
		public string HeroInformation { get; set; }
		public int HeroCost { get; set; }
		public int HeroAssetRefId { get; set; }
		public ImageAsset HeroAsset { get; set; }

		[ForeignKey("HeroRefId")]
		public ICollection<Upgrade> Upgrades { get; set; }

		[ForeignKey("HeroRefId")]
		public ICollection<PlayerHero> Players { get; set; }
	}

	public class ImageAsset
	{
		[Key]
		public int AssetId { get; set; }
		public string ImageDescription { get; set; }
		public string Base64Image { get; set; }

		//for every foreignkey you need an ICollection of those items in the code-first model
		[ForeignKey("MonsterAssetRefId")]
		public ICollection<Monster> Monsters { get; set; }

		[ForeignKey("BossAssetRefId")]
		public ICollection<Boss> Bosses { get; set; }

		[ForeignKey("HeroAssetRefId")]
		public ICollection<Hero> Heroes { get; set; }

		[ForeignKey("SceneAssetRefId")]
		public ICollection<Scene> Scenes { get; set; }
	}

	public class Upgrade
	{
		[Key]
		public int UpgradeId { get; set; }
		public string UpgradeName { get; set; }
		public int UpgradeCost { get; set; }
		public bool UpgradeIsAbility { get; set; }

		public int HeroRefId { get; set; }
		public Hero Hero { get; set; }
	}

	public class Player
	{
		[Key]
		public int PlayerId { get; set; }
		public string PlayerGuid { get; set; }
		public string PlayerName { get; set; }
		public string DeviceId { get; set; }
		public string ConnectionId { get; set; }

		[ForeignKey("PlayerRefId")]
		public ICollection<PlayerHero> Heroes { get; set; }

		//friendship collections -> there are 2 because you can see who you are friends with, and who are friends with you.
		[ForeignKey("Player1RefId")]
		public ICollection<Friendship> Friendships1 { get; set; }
		[ForeignKey("Player2RefId")]
		public ICollection<Friendship> Friendships2 { get; set; }

		[ForeignKey("Player1RefId")]
		public ICollection<MultiPlayerSession> Sessions1 { get; set; }
		[ForeignKey("Player2RefId")]
		public ICollection<MultiPlayerSession> Sessions2 { get; set; }
		[ForeignKey("Player3RefId")]
		public ICollection<MultiPlayerSession> Sessions3 { get; set; }
		[ForeignKey("Player4RefId")]
		public ICollection<MultiPlayerSession> Sessions4 { get; set; }
	}

	public class PlayerHero
	{
		[Key, Column(Order = 0)]
		public int PlayerRefId { get; set; }
		[Key, Column(Order = 1)]
		public int HeroRefId { get; set; }

		public string UnlockedTimestamp { get; set; }

		public Player Player { get; set; }
		public Hero Hero { get; set; }
	}

	public class Friendship
	{
		[Key, Column(Order = 0)]
		public int Player1RefId { get; set; }
		[Key, Column(Order = 1)]
		public int Player2RefId { get; set; }

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
	}

	public class MultiPlayerSession
	{
		//many-to-many-to-many-to-many?
		[Key]
		public int SessionId { get; set; }

		public int Player1RefId { get; set; }
		public int Player2RefId { get; set; }
		public int Player3RefId { get; set; }
		public int Player4RefId { get; set; }

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
		public Player Player3 { get; set; }
		public Player Player4 { get; set; }
	}

	public class Scene
	{
		[Key]
		public int SceneId { get; set; }	
		public string SceneName { get; set; }

		public int SceneAssetRefId { get; set; }
		public ImageAsset SceneAsset { get; set; }
	}
}