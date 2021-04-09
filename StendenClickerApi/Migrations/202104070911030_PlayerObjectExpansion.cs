namespace StendenClickerApi.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class PlayerObjectExpansion : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Players", "__SparkCoins", c => c.Long(nullable: false));
			AddColumn("dbo.Players", "__EuropeanCredits", c => c.Long(nullable: false));
			AddColumn("dbo.Players", "MonstersDefeated", c => c.Int(nullable: false));
			AddColumn("dbo.Players", "BossesDefreated", c => c.Int(nullable: false));
			DropColumn("dbo.Players", "ConnectionId");
		}

		public override void Down()
		{
			AddColumn("dbo.Players", "ConnectionId", c => c.String());
			DropColumn("dbo.Players", "BossesDefreated");
			DropColumn("dbo.Players", "MonstersDefeated");
			DropColumn("dbo.Players", "__EuropeanCredits");
			DropColumn("dbo.Players", "__SparkCoins");
		}
	}
}
