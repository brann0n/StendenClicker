namespace StendenClickerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalMigration : DbMigration
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
                        BossAsset_ImageAssetId = c.Int(),
                    })
                .PrimaryKey(t => t.BossId)
                .ForeignKey("dbo.ImageAssets", t => t.BossAsset_ImageAssetId)
                .Index(t => t.BossAsset_ImageAssetId);
            
            CreateTable(
                "dbo.ImageAssets",
                c => new
                    {
                        ImageAssetId = c.Int(nullable: false, identity: true),
                        ImageDescription = c.String(),
                        Base64Image = c.String(),
                    })
                .PrimaryKey(t => t.ImageAssetId);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        FriendshipId = c.Int(nullable: false, identity: true),
                        Player1_PlayerId = c.Int(),
                        Player2_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.FriendshipId)
                .ForeignKey("dbo.Players", t => t.Player1_PlayerId)
                .ForeignKey("dbo.Players", t => t.Player2_PlayerId)
                .Index(t => t.Player1_PlayerId)
                .Index(t => t.Player2_PlayerId);
            
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
                "dbo.Heroes",
                c => new
                    {
                        HeroId = c.Int(nullable: false, identity: true),
                        HeroName = c.String(),
                        HeroInformation = c.String(),
                        HeroCost = c.Int(nullable: false),
                        HeroAsset_ImageAssetId = c.Int(),
                    })
                .PrimaryKey(t => t.HeroId)
                .ForeignKey("dbo.ImageAssets", t => t.HeroAsset_ImageAssetId)
                .Index(t => t.HeroAsset_ImageAssetId);
            
            CreateTable(
                "dbo.Monsters",
                c => new
                    {
                        MonsterId = c.Int(nullable: false, identity: true),
                        MonsterName = c.String(),
                        BaseHealth = c.Int(nullable: false),
                        MonsterAsset_ImageAssetId = c.Int(),
                    })
                .PrimaryKey(t => t.MonsterId)
                .ForeignKey("dbo.ImageAssets", t => t.MonsterAsset_ImageAssetId)
                .Index(t => t.MonsterAsset_ImageAssetId);
            
            CreateTable(
                "dbo.Scenes",
                c => new
                    {
                        SceneId = c.Int(nullable: false, identity: true),
                        SceneName = c.String(),
                        SceneAsset_ImageAssetId = c.Int(),
                    })
                .PrimaryKey(t => t.SceneId)
                .ForeignKey("dbo.ImageAssets", t => t.SceneAsset_ImageAssetId)
                .Index(t => t.SceneAsset_ImageAssetId);
            
            CreateTable(
                "dbo.MultiPlayerSessions",
                c => new
                    {
                        MultiPlayerSessionId = c.Int(nullable: false, identity: true),
                        Player1_PlayerId = c.Int(),
                        Player2_PlayerId = c.Int(),
                        Player3_PlayerId = c.Int(),
                        Player4_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.MultiPlayerSessionId)
                .ForeignKey("dbo.Players", t => t.Player1_PlayerId)
                .ForeignKey("dbo.Players", t => t.Player2_PlayerId)
                .ForeignKey("dbo.Players", t => t.Player3_PlayerId)
                .ForeignKey("dbo.Players", t => t.Player4_PlayerId)
                .Index(t => t.Player1_PlayerId)
                .Index(t => t.Player2_PlayerId)
                .Index(t => t.Player3_PlayerId)
                .Index(t => t.Player4_PlayerId);
            
            CreateTable(
                "dbo.Upgrades",
                c => new
                    {
                        UpgradeId = c.Int(nullable: false, identity: true),
                        UpgradeName = c.String(),
                        UpgradeCost = c.Int(nullable: false),
                        UpgradeIsAbility = c.Boolean(nullable: false),
                        Hero_HeroId = c.Int(),
                    })
                .PrimaryKey(t => t.UpgradeId)
                .ForeignKey("dbo.Heroes", t => t.Hero_HeroId)
                .Index(t => t.Hero_HeroId);
            
            CreateTable(
                "dbo.HeroPlayers",
                c => new
                    {
                        Hero_HeroId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hero_HeroId, t.Player_PlayerId })
                .ForeignKey("dbo.Heroes", t => t.Hero_HeroId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId, cascadeDelete: true)
                .Index(t => t.Hero_HeroId)
                .Index(t => t.Player_PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Upgrades", "Hero_HeroId", "dbo.Heroes");
            DropForeignKey("dbo.MultiPlayerSessions", "Player4_PlayerId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player3_PlayerId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player2_PlayerId", "dbo.Players");
            DropForeignKey("dbo.MultiPlayerSessions", "Player1_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Scenes", "SceneAsset_ImageAssetId", "dbo.ImageAssets");
            DropForeignKey("dbo.Monsters", "MonsterAsset_ImageAssetId", "dbo.ImageAssets");
            DropForeignKey("dbo.Friendships", "Player2_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Friendships", "Player1_PlayerId", "dbo.Players");
            DropForeignKey("dbo.HeroPlayers", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.HeroPlayers", "Hero_HeroId", "dbo.Heroes");
            DropForeignKey("dbo.Heroes", "HeroAsset_ImageAssetId", "dbo.ImageAssets");
            DropForeignKey("dbo.Bosses", "BossAsset_ImageAssetId", "dbo.ImageAssets");
            DropIndex("dbo.HeroPlayers", new[] { "Player_PlayerId" });
            DropIndex("dbo.HeroPlayers", new[] { "Hero_HeroId" });
            DropIndex("dbo.Upgrades", new[] { "Hero_HeroId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player4_PlayerId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player3_PlayerId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player2_PlayerId" });
            DropIndex("dbo.MultiPlayerSessions", new[] { "Player1_PlayerId" });
            DropIndex("dbo.Scenes", new[] { "SceneAsset_ImageAssetId" });
            DropIndex("dbo.Monsters", new[] { "MonsterAsset_ImageAssetId" });
            DropIndex("dbo.Heroes", new[] { "HeroAsset_ImageAssetId" });
            DropIndex("dbo.Friendships", new[] { "Player2_PlayerId" });
            DropIndex("dbo.Friendships", new[] { "Player1_PlayerId" });
            DropIndex("dbo.Bosses", new[] { "BossAsset_ImageAssetId" });
            DropTable("dbo.HeroPlayers");
            DropTable("dbo.Upgrades");
            DropTable("dbo.MultiPlayerSessions");
            DropTable("dbo.Scenes");
            DropTable("dbo.Monsters");
            DropTable("dbo.Heroes");
            DropTable("dbo.Players");
            DropTable("dbo.Friendships");
            DropTable("dbo.ImageAssets");
            DropTable("dbo.Bosses");
        }
    }
}
