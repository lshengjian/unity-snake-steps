using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace QFramework.MyGame.Test
{
    public class LogicTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Test01_Move()
        {
            ISnakeMgr logic = new SnakeMgr(2, Vector2.zero);
            Assert.AreEqual(Vector2.zero, logic.GetPosition(0));
            Assert.AreEqual(Vector2.zero, logic.GetPosition(1));

            logic.HeadPosition=Vector2.right;
            Assert.AreEqual(Vector2.right, logic.GetPosition(0));
            Assert.AreEqual(Vector2.zero, logic.GetPosition(1));

        }
        [Test]
        public void Test02_Event()
        {
            ISnakeMgr logic = new SnakeMgr(1, Vector2.zero);
            logic.OnSnakeChanged += (List<Vector2> data) =>
            {
                Assert.AreEqual(Vector2.right, data[0]);
                Assert.AreEqual(Vector2.zero, data[1]);

            };
             logic.HeadPosition=Vector2.right;




        }

        [Test]
        public void Test03_HitSelf()
        {
            ISnakeMgr logic = new SnakeMgr(8, Vector2.zero);
            logic.HeadPosition=Vector2.right;
            Assert.AreEqual(Vector2.right, logic.GetPosition(0));
            Assert.AreEqual(Vector2.zero, logic.GetPosition(1));
           logic.HeadPosition= Vector2.right+Vector2.up;
            Assert.AreEqual(Vector2.right + Vector2.up, logic.GetPosition(0));
            Assert.AreEqual(Vector2.right, logic.GetPosition(1));
            logic.HeadPosition=Vector2.up;
            Assert.AreEqual(Vector2.up, logic.GetPosition(0));
            Assert.AreEqual(Vector2.right + Vector2.up, logic.GetPosition(1));
            logic.HeadPosition=Vector2.zero;
            Assert.AreEqual(Vector2.zero, logic.GetPosition(0));
            Assert.AreEqual(Vector2.up, logic.GetPosition(1));

            Assert.IsTrue(logic.IsHitSelf());


        }
    }

}

