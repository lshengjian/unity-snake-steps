using System;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.PointGame
{
    public class GamePassPanel : MonoBehaviour, IController
    {
        private void Start()
        {
          

            var gameModel = this.GetModel<IGameModel>();

            transform.Find("BestScoreText").GetComponent<Text>().text =
                "最高分数:" + gameModel.BestScore.Value;

            transform.Find("ScoreText").GetComponent<Text>().text =
                "分数:" + gameModel.Score.Value;
        }
      

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}