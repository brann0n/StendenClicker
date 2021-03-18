using Microsoft.VisualStudio.TestTools.UnitTesting;
using StendenClicker.Library.AbstractPlatform;
using StendenClicker.Library.Factory;
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
		
		List<Player> players;
		LevelGenerator levelGenerator;

		[TestInitialize]
		public void Prepare()
		{
			levelGenerator = new LevelGenerator();
			players = new List<Player>();
			players.Add(new Player { });
		}


		[TestMethod]
		public void TestBuildLevel()
		{
			IAbstractPlatform platform = levelGenerator.BuildLevel(players);
			Assert.IsTrue(platform is NormalLevel);
		}			
	}
}
