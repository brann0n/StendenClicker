using System;
using System.Collections;
using System.Collections.Generic;
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
		public virtual DbSet<Hero> Heros { get; set; }
		public virtual DbSet<ImageAsset> ImageAssets { get; set; }
		public virtual DbSet<Upgrade> Upgrades { get; set; }
	}

	public class Monster
	{
		public int MonsterId { get; set; }
		public string MonsterName { get; set; }
		public int BaseHealth { get; set; }
		public int MonsterAssetRefId { get; set; }
		public ImageAsset MonsterAsset { get; set; }
	}

	public class Boss
	{
		public int BossId { get; set; }
		public string BossName { get; set; }
		public int BaseHealth { get; set; }
		public int BossAssetRefId { get; set; }
		public ImageAsset BossAsset { get; set; }
	}

	public class Hero
	{
		public int HeroId { get; set; }
		public string HeroName { get; set; }
		public string HeroInformation { get; set; }
		public int HeroCost { get; set; }

		[ForeignKey("HeroRefId")]
		public ICollection<Upgrade> Upgrades { get; set; }
	}

	public class ImageAsset
	{
		public int AssetId { get; set; }
		public string ImageDescription { get; set; }
		public string Base64Image { get; set; }

		//for every foreignkey you need an ICollection of those items in the code-first model
		[ForeignKey("MonsterAssetRefId")]
		public ICollection<Monster> Monsters { get; set; }

		[ForeignKey("BossAssetRefId")]
		public ICollection<Boss> Bosses { get; set; }
	}

	public class Upgrade
	{
		public int UpgradeId { get; set; }
		public string UpgradeName { get; set; }
		public int UpgradeCost { get; set; }
		public bool UpgradeIsAbility { get; set; }

		public int HeroRefId { get; set; }
		public Hero Hero { get; set; }
	}
}