using System.Collections.Generic;
using UnityEngine;

namespace QFramework.MyGame
{
    public interface ISysSpawn : ISystem
    {
      public  List<BindableProperty<Vector2>> FoodBindPositions{get;}
         public Vector2 GetFreePosition();
          public ISnakeMgr GetSnakeMgr(string name);
            public void ResetSnakes(bool gotoZero);//just for test
           
        //   void MakeFoods(CmdMakeFoods cmd);
        //  void MakeSnakes(CmdMakeSnakes cmd);
    }
    public class CmdSwitchThroughWalls : AbstractCommand
    {
        protected override void OnExecute()
        {
            var md = this.GetModel<GameModel>();
            md.canThroughWalls.Value = !md.canThroughWalls.Value;
        }

    }

    //  public class CmdMakeFoods : AbstractCommand
    // {
    //     protected override void OnExecute()
    //     {
    //         this.GetSystem<ISysSpawn>().MakeFoods(this);
    //     }

    // }


    public class SysSpawn : AbstractSystem, ISysSpawn
    {
        FoodModel foodModel;
        GameModel mGameModel;

        List<BindableProperty<Vector2>> mFoodBindPositions=new List<BindableProperty<Vector2>>();
        Dictionary<string, ISnakeMgr> mSnakes = new Dictionary<string, ISnakeMgr>();
         public  List<BindableProperty<Vector2>> FoodBindPositions=>mFoodBindPositions;
        public ISnakeMgr GetSnakeMgr(string name){
            return mSnakes[name];
        }
        protected override void OnInit()
        {

            foodModel = this.GetModel<FoodModel>();
            // sysMove = this.GetSystem<ISysMove>();
            mGameModel = this.GetModel<GameModel>();
            MakeFoods();
            foreach (var name in mGameModel.PlayerNames)
            {
                var pd = mGameModel.GetPlayerData(name);
                var pos = GetFreePosition();
                pd.InitPosition = pos;
                mSnakes[name] = new SnakeMgr(mGameModel.NumInitSnakeSegments, pos);
               
            }
        }
               public void ResetSnakes(bool gotoZero)
        {
            foreach (var name in mGameModel.PlayerNames)
            {
                var pd = mGameModel.GetPlayerData(name);
                pd.Life.Value = 3;
                if (gotoZero) pd.InitPosition = Vector2.zero;
                mSnakes[name].Reset(pd.InitPosition);
            }
        }
        bool IsOccupies(int x, int y)
        {
            foreach (var s in mSnakes.Values)
            {
                if (s.IsOccupies(x, y))
                    return true;
            }
            foreach (var pos in mFoodBindPositions)
            {
                if (pos.Value == new Vector2(x, y))
                    return true;
            }
            return false;
        }
        public Vector2 GetFreePosition()
        {
            Bounds bounds = mGameModel.SiteBounds;
            // Pick a random position inside the bounds
            // Round the values to ensure it aligns with the grid
            int x = Mathf.RoundToInt(Random.Range(bounds.min.x + 1, bounds.max.x - 1));
            int y = Mathf.RoundToInt(Random.Range(bounds.min.y + 1, bounds.max.y - 1));

            // Prevent the food from spawning on the snake
            while (IsOccupies(x, y))
            {
                x++;
                if (x > bounds.max.x)
                {
                    x = Mathf.RoundToInt(bounds.min.x);
                    y++;
                    if (y > bounds.max.y)
                        y = Mathf.RoundToInt(bounds.min.y);
                }
            }
            return new Vector2(x, y);

        }


        public void MakeFoods()
        {
            mFoodBindPositions = new List<BindableProperty<Vector2>>();
            for (int i = 0; i < foodModel.NumFood; i++)
            {
                Vector2 pos = GetFreePosition();
                foodModel.SetPositionAt(i, pos);
                mFoodBindPositions.Add(foodModel.GetBindPropAt(i));
            }
            // EventFoodSpawned e=new EventFoodSpawned(){data=data};
            // this.SendEvent(e);

        }



    }
}