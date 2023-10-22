using UnityEngine;

namespace QFramework.MyGame
{
    public interface IScoreSystem : ISystem
    {
        int TotalScore {get;}
     //   BindableProperty<int> PlayerScore(string name);
        void ResetPlayerScore(string name);
     //   void AddScore(string name,int score);
    }

    public class ScoreSystem : AbstractSystem,IScoreSystem
    {
        GameModel gameModel;
        protected override void OnInit()
        {
            gameModel = this.GetModel<GameModel>();
            
            this.RegisterEvent<EventGamePassed>(e =>
            {
             

                if (gameModel.Score.Value > gameModel.BestScore.Value)
                {
                    gameModel.BestScore.Value = gameModel.Score.Value;
                    
                    Debug.Log("新纪录");
                }
            });

         
        }
        public int TotalScore=>gameModel.Score.Value;
  
        // public  BindableProperty<int> PlayerScore(string name){
        //     return gameModel.GetScoreByName(name);
        // }
       public void ResetPlayerScore(string name){
        gameModel.Score.Value=0;
       }
        // public  void AddScore(string name,int score){
        //     gameModel.GetScoreByName(name).Value+=score;
        //     gameModel.Score.Value+=score;
        //     if (gameModel.Score.Value>gameModel.PassScore)
        //     this.SendEvent<GamePassEvent>();
        // }
    }
}