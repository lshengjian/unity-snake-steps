using System.Collections.Generic;
using UnityEngine;
namespace QFramework.MyGame
{
     public struct EventSnakeMoved 
    {
        public string name;
        public List<Vector2> data;

    }



    public struct EventGamePassed 
    {

    }
    public struct EventGameOver 
    {

    }
}