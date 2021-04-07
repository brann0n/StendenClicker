namespace StendenClickerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Upgrades", "PlayerHero_PlayerHeroId", "dbo.PlayerHeroes");
            DropIndex("dbo.Upgrades", new[] { "PlayerHero_PlayerHeroId" });
            CreateTable(
                "dbo.UpgradePlayerHeroes",
                c => new
                    {
                        Upgrade_UpgradeId = c.Int(nullable: false),
                        PlayerHero_PlayerHeroId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Upgrade_UpgradeId, t.PlayerHero_PlayerHeroId })
                .ForeignKey("dbo.Upgrades", t => t.Upgrade_UpgradeId, cascadeDelete: true)
                .ForeignKey("dbo.PlayerHeroes", t => t.PlayerHero_PlayerHeroId, cascadeDelete: true)
                .Index(t => t.Upgrade_UpgradeId)
                .Index(t => t.PlayerHero_PlayerHeroId);
            
            DropColumn("dbo.Upgrades", "PlayerHero_PlayerHeroId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Upgrades", "PlayerHero_PlayerHeroId", c => c.Int());
            DropForeignKey("dbo.UpgradePlayerHeroes", "PlayerHero_PlayerHeroId", "dbo.PlayerHeroes");
            DropForeignKey("dbo.UpgradePlayerHeroes", "Upgrade_UpgradeId", "dbo.Upgrades");
            DropIndex("dbo.UpgradePlayerHeroes", new[] { "PlayerHero_PlayerHeroId" });
            DropIndex("dbo.UpgradePlayerHeroes", new[] { "Upgrade_UpgradeId" });
            DropTable("dbo.UpgradePlayerHeroes");
            CreateIndex("dbo.Upgrades", "PlayerHero_PlayerHeroId");
            AddForeignKey("dbo.Upgrades", "PlayerHero_PlayerHeroId", "dbo.PlayerHeroes", "PlayerHeroId");
        }
    }
}
