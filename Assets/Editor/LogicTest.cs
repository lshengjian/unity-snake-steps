using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LogicTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void Test01_Move()
    {
        ISnakeController logic=new SnakeController(2);
        Assert.AreEqual(Vector2.zero,logic.GetPosition(0));
        Assert.AreEqual(Vector2.zero,logic.GetPosition(1));

        logic.Move(Vector2Int.right);
        Assert.AreEqual(Vector2.right,logic.GetPosition(0));
        Assert.AreEqual(Vector2.zero,logic.GetPosition(1));

    }

    [Test]
    public void Test02_Over()
    {
        ISnakeController logic=new SnakeController(8);
        logic.Move(Vector2Int.right);
        Assert.AreEqual(Vector2.right,logic.GetPosition(0));
        Assert.AreEqual(Vector2.zero,logic.GetPosition(1));
        logic.Move(Vector2Int.up);
        Assert.AreEqual(Vector2.right+Vector2.up,logic.GetPosition(0));
        Assert.AreEqual(Vector2.right,logic.GetPosition(1));
        logic.Move(Vector2Int.left);
        Assert.AreEqual(Vector2.up,logic.GetPosition(0));
        Assert.AreEqual(Vector2.right+Vector2.up,logic.GetPosition(1));
        logic.Move(Vector2Int.down);
        Assert.AreEqual(Vector2.zero,logic.GetPosition(0));
        Assert.AreEqual(Vector2.up,logic.GetPosition(1));

        Assert.IsTrue(logic.IsOver());
        

    }
}
