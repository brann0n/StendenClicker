namespace StendenClickerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Heroes", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.Heroes", new[] { "Player_PlayerId" });
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
            
            DropColumn("dbo.Heroes", "Player_PlayerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Heroes", "Player_PlayerId", c => c.Int());
            DropForeignKey("dbo.HeroPlayers", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.HeroPlayers", "Hero_HeroId", "dbo.Heroes");
            DropIndex("dbo.HeroPlayers", new[] { "Player_PlayerId" });
            DropIndex("dbo.HeroPlayers", new[] { "Hero_HeroId" });
            DropTable("dbo.HeroPlayers");
            CreateIndex("dbo.Heroes", "Player_PlayerId");
            AddForeignKey("dbo.Heroes", "Player_PlayerId", "dbo.Players", "PlayerId");
        }
    }
}
