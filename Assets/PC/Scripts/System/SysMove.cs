using System.Collections.Generic;

using UnityEngine;
namespace QFramework.MyGame
{

    public class CmdMove : AbstractCommand
    {

        public string name;
        public Vector2 direction;
        public CmdMove(string n, Vector2 dir)
        {
            name = n;
            direction = dir;
        }
        protected override void OnExecute()
        {
            this.GetSystem<ISysMove>().InvokeCommand(this);
        }
    }
    public interface ISysMove : ISystem
    {
        public void InvokeCommand(CmdMove cmd);


        public List<Vector2> GetPositions(string name);
        //  public Vector2 GetFreePosition();
    }
    public class SysMove : AbstractSystem, ISysMove//,IOnEvent<EventSnakeMoved>
    {
        GameModel mGameModel;
        Dictionary<string, List<Vector2>> mSnakePositions = new Dictionary<string, List<Vector2>>();
        ISysSpawn mSysSpawn;
        protected override void OnInit()
        {
            mGameModel = this.GetModel<GameModel>();
            mSysSpawn = this.GetSystem<ISysSpawn>();
            foreach (var name in mGameModel.PlayerNames)
            {
                mSnakePositions[name] = new List<Vector2>();
            }
        }
        public List<Vector2> GetPositions(string name)
        {
            return mSnakePositions[name];
        }

        public void InvokeCommand(CmdMove cmd)
        {
            var pd = mGameModel.GetPlayerData(cmd.name);
            ISnakeMgr snake = mSysSpawn.GetSnakeMgr(cmd.name);
            var pos = snake.HeadPosition + cmd.direction;

            if (!mGameModel.SiteBounds.Contains(new Vector3(pos.x, pos.y)))
            {
                if (mGameModel.canThroughWalls.Value)
                {
                    pos = Traverse(pos);
                }
                else
                {
                    pos = pd.InitPosition;
                    pd.Life.Value -= 1;
                }

            }
            snake.HeadPosition = pos;
            var data = new List<Vector2>();
            for (int i = 0; i < snake.NumSegments; i++)
            {
                data.Add(snake.GetPosition(i));
            }
            mSnakePositions[cmd.name].Clear();
            mSnakePositions[cmd.name].AddRange(data);


            if (snake.IsHitSelf())

                this.SendEvent<EventGameOver>();
            else
            {
                EventSnakeMoved e = new EventSnakeMoved() { name = cmd.name, data = data };
                CheckHitFood(cmd.name, data);
                this.SendEvent(e);
            }

        }

        void CheckHitFood(string name, List<Vector2> data)
        {
            var foodModel = this.GetModel<FoodModel>();
            for (int i = 0; i < foodModel.NumFood; i++)
            {
                var posProp = foodModel.GetBindPropAt(i);
                foreach (var p in data)
                {
                    if (p == posProp.Value)
                    {
                        //Debug.Log("Hit food");
                        posProp.Value = mSysSpawn.GetFreePosition();
                        ISnakeMgr snake = mSysSpawn.GetSnakeMgr(name);
                        snake.Grow();
                        var pd = mGameModel.GetPlayerData(name);
                        pd.Score.Value += 10;
                        break;
                    }
                }
            }

        }
        Vector2 Traverse(Vector2 pos)
        {


            float w = mGameModel.SiteBounds.size.x;
            float h = mGameModel.SiteBounds.size.y;
            var p1 = mGameModel.SiteBounds.min;
            float x = pos.x;
            float y = pos.y;

            x = Mathf.Repeat(x - p1.x, w) + p1.x; //0~w-1
            y = Mathf.Repeat(y - p1.y, h) + p1.y;
            return new Vector2(x, y);

        }

    }
    // bool CheckHitBody(string name, List<Vector2> data) //后端检擦效率太低，并且没有考虑CoolDown机制
    // {
    //     ISnakeMgr snake = mSysSpawn.GetSnakeMgr(name);
    //     if (snake.IsHitSelf())
    //     {
    //         this.SendEvent<EventGameOver>();
    //         return true;
    //     }
    //     else
    //     {
    //         foreach (var pos in data)
    //         {
    //             foreach (var other in mGameModel.PlayerNames)
    //             {
    //                 if (name == other) continue;
    //                 ISnakeMgr snake2 = mSysSpawn.GetSnakeMgr(other);
    //                 if (snake2.IsOccupies(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)))
    //                 {
    //                     mGameModel.Score.Value -= 5;
    //                     return true;
    //                 }
    //             }
    //         }
    //     }
    //     return false;
    // }
}