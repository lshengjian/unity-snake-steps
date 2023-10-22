using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace QFramework.MyGame.Test
{
    [TestFixture]
    public class GameTest
    {
        const string playerName="Player1";
        IArchitecture game;
        ISysMove sys;
        [SetUp]
        public void SetUp()
        {
            game = MockGame.Interface;
            sys = game.GetSystem<ISysMove>();
            game.GetSystem<ISysSpawn>().ResetSnakes(true);


        }
        // A Test behaves as an ordinary method
        [Test]
        public void Test01_FoodBindPosition()
        {

            var fd = game.GetModel<FoodModel>();
            var pos = Vector2.zero;
            BindableProperty<Vector2> food = fd.GetBindPropAt(0);

            food.Register((Vector2 p) =>
            {
                pos = p;
                Assert.AreEqual(pos, Vector2.one);
            });

            food.Value = Vector2.one;


        }

        [Test]
        public void Test02_ExeCommand()
        {

            var data = game.GetModel<GameModel>();
            var pd = data.GetPlayerData(playerName);

            CmdMove cmd = new CmdMove(playerName, new Vector2(3, 0));
            var poss = sys.GetPositions(playerName);
            game.SendCommand(cmd);
            Assert.AreEqual(poss[0].x, 3.0f);
        }
        [Test]
        public void Test03_PassWall()
        {
           
            var data = game.GetModel<GameModel>();
            data.canThroughWalls.Value = true;
            var pd = data.GetPlayerData(playerName);
            var poss = sys.GetPositions(playerName);

            CmdMove cmd = new CmdMove(playerName, new Vector2(31, 0));
            game.SendCommand(cmd);
            Assert.AreEqual(-29.0f,poss[0].x);
            Assert.AreEqual(pd.Life.Value, 3);


        }

        [Test]
        public void Test04_LostLife()
        {
            var data = game.GetModel<GameModel>();
            data.canThroughWalls.Value = false;
            var pd = data.GetPlayerData(playerName);
            CmdMove cmd = new CmdMove(playerName, new Vector2(100, 0));
            game.SendCommand(cmd);
            Assert.AreEqual(pd.Life.Value, 2);
        }



    }
}
