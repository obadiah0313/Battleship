using System;
using NUnit.Framework;
using SwinGameSDK;
namespace MyGame
{
	[TestFixture]
	public class BattleshipUnitTesting
	{
		[Test]
		public void AddPlayerTest ()
		{
			BattleShipsGame testGame = new BattleShipsGame ();
			Player p1 = new Player (testGame);
			testGame.AddDeployedPlayer (p1);
			Assert.AreEqual (p1, testGame.Player);
		}

		[Test]
		public void IsDestroyedTest ()
		{
			Ship testShip = new Ship (ShipName.AircraftCarrier);
			for (int i = 0; i < 5; i++) { testShip.Hit (); }
			bool expected = true;
			bool actual = testShip.IsDestroyed;
			Assert.AreEqual (expected, actual);
		}
	}
}
