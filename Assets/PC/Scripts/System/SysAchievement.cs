using UnityEngine;

namespace QFramework.MyGame
{
    public interface ISysAchievement : ISystem
    {
        public int PassScore { get; }

    }

    public class SysAchievement : AbstractSystem, ISysAchievement
    {
        private int mPassScore = 500;
        GameModel mGameModel;
        protected override void OnInit()
        {
            mGameModel = this.GetModel<GameModel>();
            mGameModel.Score.Register(v =>
            {
                if (v >= mGameModel.BestScore.Value)
                    mGameModel.BestScore.Value = v;
            });
            foreach (var name in mGameModel.PlayerNames)
            {
                var pd = mGameModel.GetPlayerData(name);
                pd.Score.Register(SumScores);
                pd.Life.Register(v =>
                {
                    if (v < 1) this.SendEvent<EventGameOver>();
                });

            }


        }
        public int PassScore => mPassScore;
        void SumScores(int score)
        {
          int  totalScore = 0;
            foreach (var name in mGameModel.PlayerNames)
            {
                var pd = mGameModel.GetPlayerData(name);
                totalScore += pd.Score.Value;
            }
            mGameModel.Score.Value = totalScore;
            if (totalScore >= PassScore)
            {
                this.SendEvent<EventGamePassed>();
            }

        }

    }
}