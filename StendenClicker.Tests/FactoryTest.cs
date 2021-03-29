using Microsoft.VisualStudio.TestTools.UnitTesting;
using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractPlatform;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Models;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StendenClicker.Tests
{
	[TestClass]
	public class FactoryTest
    {
		LevelGenerator levelGenerator;

		[TestInitialize]
		public void Prepare()
		{
			levelGenerator = new LevelGenerator();
		}


		[TestMethod]
		public void TestBuildLevel()
		{
			int stateNumber = 10;
			List<Player> players = new List<Player>();
			for (int i = 0; i < 4; i++)
			{
				players.Add(new Player
				{
					connectionId = "01",
					deviceId = $"{i}",
					State = new PlayerState { MonstersDefeated = stateNumber },
					UserId = Guid.NewGuid(),
					Username = $"Test{i}",
					Wallet = new PlayerCurrency { }
				});
			}

			GamePlatform platform = levelGenerator.BuildLevel(players);
			if(stateNumber % LevelGenerator.LevelsUntilBoss == 0)
			{
				Assert.IsTrue(platform is BossLevel);

				Assert.IsTrue(platform.Monster is BossScene);

				Assert.IsTrue(platform.Scene is Boss);
			}
			else
			{
				Assert.IsTrue(platform is NormalLevel);

				Assert.IsTrue(platform.Scene is NormalScene);

				Assert.IsTrue(platform.Monster is Normal);
			}
		}			

		[TestMethod]
		public void TestRandomizer()
        {
			Random r = new Random();
			bool gotWielklem = false;
			int count = 0;
            while (!gotWielklem)
            {
				int random = r.Next(1, 13);
				if(random == 12)
                {
					gotWielklem = true;
					continue;
                }
				count++;
            }
			Console.Write(count);
			Assert.IsTrue(count < 50);
        }
	}
}
