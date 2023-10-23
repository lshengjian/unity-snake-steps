using UnityEngine;
using TMPro;

namespace QFramework.MyGame
{
    public class GameInfo : MonoBehaviour, IController
    {
      //  private ICountDownSystem mCountDownSystem;
      public TextMeshProUGUI scoreText;
       public TextMeshProUGUI bestScoreText;
        private GameModel mGameModel;

        private void Awake()
        {
        //    mCountDownSystem = this.GetSystem<ICountDownSystem>();

            mGameModel = this.GetModel<GameModel>();

  
            mGameModel.Score.RegisterWithInitValue(OnScoreValueChanged)
            .UnRegisterWhenGameObjectDestroyed(gameObject);

             mGameModel.BestScore.RegisterWithInitValue(OnBestScoreValueChanged)
            .UnRegisterWhenGameObjectDestroyed(gameObject);


           
        }

       private void OnScoreValueChanged(int score)
        {
           scoreText.text = "Score:" + score;
        }

        private void OnBestScoreValueChanged(int score)
        {
           bestScoreText.text = "Best:" + score;
        }

       

        private void OnDestroy()
        {
            mGameModel = null;
        }

        public IArchitecture GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }
    }
}