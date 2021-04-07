namespace StendenClickerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpgradesAndHeros : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HeroPlayers", "Hero_HeroId", "dbo.Heroes");
            DropForeignKey("dbo.HeroPlayers", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.HeroPlayers", new[] { "Hero_HeroId" });
            DropIndex("dbo.HeroPlayers", new[] { "Player_PlayerId" });
            CreateTable(
                "dbo.PlayerHeroes",
                c => new
                    {
                        PlayerHeroId = c.Int(nullable: false, identity: true),
                        HeroUpgradeLevel = c.Int(nullable: false),
                        SpecialUpgradeLevel = c.Int(nullable: false),
                        Hero_HeroId = c.Int(),
                        Player_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.PlayerHeroId)
                .ForeignKey("dbo.Heroes", t => t.Hero_HeroId)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId)
                .Index(t => t.Hero_HeroId)
                .Index(t => t.Player_PlayerId);
            
            AddColumn("dbo.Upgrades", "PlayerHero_PlayerHeroId", c => c.Int());
            CreateIndex("dbo.Upgrades", "PlayerHero_PlayerHeroId");
            AddForeignKey("dbo.Upgrades", "PlayerHero_PlayerHeroId", "dbo.PlayerHeroes", "PlayerHeroId");
            DropTable("dbo.HeroPlayers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HeroPlayers",
                c => new
                    {
                        Hero_HeroId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hero_HeroId, t.Player_PlayerId });
            
            DropForeignKey("dbo.PlayerHeroes", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Upgrades", "PlayerHero_PlayerHeroId", "dbo.PlayerHeroes");
            DropForeignKey("dbo.PlayerHeroes", "Hero_HeroId", "dbo.Heroes");
            DropIndex("dbo.Upgrades", new[] { "PlayerHero_PlayerHeroId" });
            DropIndex("dbo.PlayerHeroes", new[] { "Player_PlayerId" });
            DropIndex("dbo.PlayerHeroes", new[] { "Hero_HeroId" });
            DropColumn("dbo.Upgrades", "PlayerHero_PlayerHeroId");
            DropTable("dbo.PlayerHeroes");
            CreateIndex("dbo.HeroPlayers", "Player_PlayerId");
            CreateIndex("dbo.HeroPlayers", "Hero_HeroId");
            AddForeignKey("dbo.HeroPlayers", "Player_PlayerId", "dbo.Players", "PlayerId", cascadeDelete: true);
            AddForeignKey("dbo.HeroPlayers", "Hero_HeroId", "dbo.Heroes", "HeroId", cascadeDelete: true);
        }
    }
}
