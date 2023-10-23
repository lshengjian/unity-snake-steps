using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
namespace QFramework.MyGame
{
    public class SnakeGame : MonoBehaviour, IController
    {
     //   public Collider2D gridArea;
       
        public GameObject PassUI;
         public GameObject OverUI;
        private GameObject[] m_walls;
        GameModel mGameModel;
   
    
        private void Awake()
        {

            mGameModel=this.GetModel<GameModel>();
            this.RegisterEvent<EventGamePassed>(e =>
            {
              //  Debug.Log("Game Pass! ");
                PassUI.SetActive(true);
                OverUI.SetActive(false);
                Time.timeScale=0;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<EventGameOver>(e =>
            {
              //  Debug.Log("Game Pass! ");
                OverUI.SetActive(true);
                PassUI.SetActive(false);
                Time.timeScale=0;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
           
             
        }
       
        public void Replay()
        {
            Time.timeScale=1;
            this.GetSystem<ISysSpawn>().ResetSnakes(false);
            SceneManager.LoadScene("PlayLocal");
        }
       
        private void Start()
        {
            m_walls = GameObject.FindGameObjectsWithTag("Wall");
             mGameModel.canThroughWalls.RegisterWithInitValue(SetWallColor)
             .UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        
       
        void SetWallColor(bool canThroughWalls)
        {
            foreach (GameObject wall in m_walls)
            {
                SpriteRenderer sp = wall.GetComponent<SpriteRenderer>();
                sp.color =canThroughWalls ? Color.green : Color.gray;
            }

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                this.SendCommand<CmdSwitchThroughWalls>();
            }
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return SnakeArchitecture.Interface;
        }

    }
}
