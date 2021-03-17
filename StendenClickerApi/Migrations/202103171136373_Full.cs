namespace StendenClickerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Full : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bosses",
                c => new
                    {
                        BossId = c.Int(nullable: false, identity: true),
                        BossName = c.String(),
                        BaseHealth = c.Int(nullable: false),
                        BossAssetRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BossId)
                .ForeignKey("dbo.ImageAssets", t => t.BossAssetRefId, cascadeDelete: true)
                .Index(t => t.BossAssetRefId);
            
            CreateTable(
                "dbo.ImageAssets",
                c => new
                    {
                        AssetId = c.Int(nullable: false, identity: true),
                        ImageDescription = c.String(),
                        Base64Image = c.String(),
                    })
                .PrimaryKey(t => t.AssetId);
            
            CreateTable(
                "dbo.Heroes",
                c => new
                    {
                        HeroId = c.Int(nullable: false, identity: true),
                        HeroName = c.String(),
                        HeroInformation = c.String(),
                        HeroCost = c.Int(nullable: false),
                        HeroAssetRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HeroId)
                .ForeignKey("dbo.ImageAssets", t => t.HeroAssetRefId, cascadeDelete: true)
                .Index(t => t.HeroAssetRefId);
            
            CreateTable(
                "dbo.PlayerHeroes",
                c => new
                    {
                        PlayerRefId = c.Int(nullable: false),
                        HeroRefId = c.Int(nullable: false),
                        UnlockedTimestamp = c.String(),
                    })
                .PrimaryKey(t => new { t.PlayerRefId, t.HeroRefId })
                .ForeignKey("dbo.Heroes", t => t.HeroRefId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.PlayerRefId, cascadeDelete: true)
                .Index(t => t.PlayerRefId)
                .Index(t => t.HeroRefId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        PlayerGuid = c.String(),
                        PlayerName = c.String(),
                        DeviceId = c.String(),
                        ConnectionId = c.String(),
                    })
                .PrimaryKey(t => t.PlayerId);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Player1RefId = c.Int(nullable: false),
                        Player2RefId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(),
                        Player_PlayerId1 = c.Int(),
                    })
                .PrimaryKey(t => new { t.Player1RefId, t.Player2RefId })
                .ForeignKey("dbo.Players", t => t.Player1RefId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player2RefId, cascadeDelete: false)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId1)
                .Index(t => t.Player1RefId)
                .Index(t => t.Player2RefId)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Player_PlayerId1);
            
            CreateTable(
                "dbo.MultiPlayerSessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        Player1RefId = c.Int(nullable: false),
                        Player2RefId = c.Int(nullable: true),
                        Player3RefId = c.Int(nullable: true),
                        Player4RefId = c.Int(nullable: true),
                        Player_PlayerId = c.Int(),
                        Player_PlayerId1 = c.Int(),
                        Player_PlayerId2 = c.Int(),
                        Player_PlayerId3 = c.Int(),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.Players", t => t.Player1RefId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player2RefId, cascadeDelete: false)
                .ForeignKey("dbo.Players", t => t.Player3RefId, cascadeDelete: false)
                .ForeignKey("dbo.Players", t => t.Player4RefId, cascadeDelete: false)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId1)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId2)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId3)
                .Index(t => t.Player1RefId)
                .Index(t => t.Player2RefId)
                .Index(t => t.Player3RefId)
                .Index(t => t.Player4RefId)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Player_PlayerId1)
                .Index(t => t.Player_PlayerId2)
                .Index(t => t.Player_PlayerId3);
            
            CreateTable(
                "dbo.Upgrades",
                c => new
                    {
                        UpgradeId = c.Int(nullable: false, identity: true),
                        UpgradeName = c.String(),
                        UpgradeCost = c.Int(nullable: false),
                        UpgradeIsAbility = c.Boolean(nullable: false),
                        HeroRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UpgradeId)
                .ForeignKey("dbo.Heroes", t => t.HeroRefId, cascadeDelete: true)
                .Index(t => t.HeroRefId);
            
            CreateTable(
                "dbo.Monsters",
                c => new
                    {
                        MonsterId = c.Int(nullable: false, identity: true),
                        MonsterName = c.String(),
                        BaseHealth = c.Int(nullable: false),
                        MonsterAssetRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MonsterId)
                .ForeignKey("dbo.ImageAssets", t => t.MonsterAssetRefId, cascadeDelete: true)
                .Index(t => t.MonsterAssetRefId);
            
            CreateTable(
                "dbo.Scenes",
                c => new
                    {
                        SceneId = c.Int(nullable: false, identity: true),
                        SceneName = c.String(),
                        SceneAssetRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SceneId)
                .ForeignKey("dbo.ImageAssets", t => t.SceneAssetRefId, cascadeDelete: true)
                .Index(t => t.SceneAssetRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bosses", "BossAssetRefId", "dbo.ImageAssets");
            DropForeignKey("dbo.Scenes", "SceneAssetRefId", "dbo.ImageAssets");
            DropForeignKey("dbo.Monsters", "MonsterAssetRefId", "dbo.ImageAssets");
            DropForeignKey("dbo.Upgrades", "HeroRefId", "dbo.Heroes");
            DropForeignKey("dbo.PlayerHeroes", "PlayerRefId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player_PlayerId3", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player_PlayerId2", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player_PlayerId1", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player4RefId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player3RefId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player2RefId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player1RefId", "dbo.Players");
            DropForeignKey("dbo.Friendships", "Player_PlayerId1", "dbo.Players");
            DropForeignKey("dbo.Friendships", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Friendships", "Player2RefId", "dbo.Players");
            DropForeignKey("dbo.Friendships", "Player1RefId", "dbo.Players");
            DropForeignKey("dbo.PlayerHeroes", "HeroRefId", "dbo.Heroes");
            DropForeignKey("dbo.Heroes", "HeroAssetRefId", "dbo.ImageAssets");
            DropIndex("dbo.Scenes", new[] { "SceneAssetRefId" });
            DropIndex("dbo.Monsters", new[] { "MonsterAssetRefId" });
            DropIndex("dbo.Upgrades", new[] { "HeroRefId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player_PlayerId3" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player_PlayerId2" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player_PlayerId1" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player_PlayerId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player4RefId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player3RefId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player2RefId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player1RefId" });
            DropIndex("dbo.Friendships", new[] { "Player_PlayerId1" });
            DropIndex("dbo.Friendships", new[] { "Player_PlayerId" });
            DropIndex("dbo.Friendships", new[] { "Player2RefId" });
            DropIndex("dbo.Friendships", new[] { "Player1RefId" });
            DropIndex("dbo.PlayerHeroes", new[] { "HeroRefId" });
            DropIndex("dbo.PlayerHeroes", new[] { "PlayerRefId" });
            DropIndex("dbo.Heroes", new[] { "HeroAssetRefId" });
            DropIndex("dbo.Bosses", new[] { "BossAssetRefId" });
            DropTable("dbo.Scenes");
            DropTable("dbo.Monsters");
            DropTable("dbo.Upgrades");
            DropTable("dbo.MultiPlayerSessions");
            DropTable("dbo.Friendships");
            DropTable("dbo.Players");
            DropTable("dbo.PlayerHeroes");
            DropTable("dbo.Heroes");
            DropTable("dbo.ImageAssets");
            DropTable("dbo.Bosses");
        }
    }
}
