using UnityEngine;
using TMPro;
namespace QFramework.MyGame
{
    public class PlayerInfo : MonoBehaviour, IController
    {
       public TextMeshProUGUI scoreText;
        public TextMeshProUGUI lifeText;
       public string playerName{get;set;} = "Player1";

       
        void Start()
        {
  
            var mGameModel=this.GetModel<GameModel>();
           var pd= mGameModel.GetPlayerData(playerName);
           scoreText.color=pd.config.color;
           lifeText.color=pd.config.color;
           pd.Score.RegisterWithInitValue((int score)=>{
            scoreText.text="Score:"+score;

           }).UnRegisterWhenGameObjectDestroyed(gameObject);
           pd.Life.RegisterWithInitValue((int life)=>{
            lifeText.text="Life :"+life;
           }).UnRegisterWhenGameObjectDestroyed(gameObject);

        }
       
        
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }

    }
}