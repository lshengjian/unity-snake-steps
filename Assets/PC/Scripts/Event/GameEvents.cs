using System.Collections.Generic;
using UnityEngine;
namespace QFramework.MyGame
{
     public struct EventSnakeMoved 
    {
        public string name;
        public List<Vector2> data;

    }

     public struct EventFoodSpawned 
    {
     
        public List<BindableProperty<Vector2>> data;

    }
    public struct EventGamePassed 
    {

    }
}