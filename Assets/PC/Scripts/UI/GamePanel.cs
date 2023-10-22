using UnityEngine;
using TMPro;

namespace QFramework.MyGame
{
    public class GamePanel : MonoBehaviour, IController
    {
      //  private ICountDownSystem mCountDownSystem;
      public TextMeshProUGUI scoreText;
        private GameModel mGameModel;

        private void Awake()
        {
        //    mCountDownSystem = this.GetSystem<ICountDownSystem>();

            mGameModel = this.GetModel<GameModel>();

  
            mGameModel.Score.Register(OnScoreValueChanged);

            // 第一次需要调用一下
            OnScoreValueChanged(mGameModel.Score.Value);
        }

      

        private void OnScoreValueChanged(int score)
        {
           scoreText.text = "Score:" + score;
        }

        private void Update()
        {
            // 每 20 帧 更新一次
            if (Time.frameCount % 20 == 0)
            {
                // transform.Find("CountDownText").GetComponent<Text>().text =
                //     mCountDownSystem.CurrentRemainSeconds + "s";

                // mCountDownSystem.Update();
            }
        }

        private void OnDestroy()
        {
  
            mGameModel.Score.UnRegister(OnScoreValueChanged);
            mGameModel = null;
            //mCountDownSystem = null;
        }

        public IArchitecture GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }
    }
}