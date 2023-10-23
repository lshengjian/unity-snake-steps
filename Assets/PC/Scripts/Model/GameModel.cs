using System.Collections.Generic;
using UnityEngine;

namespace QFramework.MyGame
{

    public class PlayerData
    {
       

        public Vector2 InitPosition = Vector2.zero;
        public RoleData config;
        //   public BindableProperty<Color> Color { get; } = new BindableProperty<Color>();
        public BindableProperty<int> Life { get; } = new BindableProperty<int>()
        {
            Value = 3
        };
        public BindableProperty<int> Score { get; } = new BindableProperty<int>();
       // public BindableProperty<int> BestScore { get; } = new BindableProperty<int>();
    }

    public class GameModel : AbstractModel
    {
        public int NumInitSnakeSegments { get; } = 4;
        public Bounds SiteBounds => new Bounds(Vector3.zero, new Vector3(60, 30));

        public string[] PlayerNames { get; } = { "Player1", "Player2" };
        public int NumPlayer => PlayerNames.Length;
        public BindableProperty<bool> canThroughWalls { get; } = new BindableProperty<bool>(true);

        Dictionary<string, PlayerData> mPlayers = new Dictionary<string, PlayerData>();


        public BindableProperty<int> Score { get; } = new BindableProperty<int>();
        public BindableProperty<int> BestScore { get; } = new BindableProperty<int>();

        protected override void OnInit()
        {
            var storage = this.GetUtility<IStorage>();

            BestScore.Value = storage.LoadInt(nameof(BestScore), 0);
            BestScore.Register(v => storage.SaveInt(nameof(BestScore), v));
            foreach (var n in PlayerNames)
            {
                var pd = new PlayerData();
                mPlayers[n] = pd;
                mPlayers[n].config = Resources.Load<RoleData>(n);
            
            }
       

        }
        public PlayerData GetPlayerData(string name)
        {
            return mPlayers[name];

        }
    }
}