using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
namespace QFramework.MyGame
{
    public class SnakeGame : MonoBehaviour, IController
    {
     //   public Collider2D gridArea;
       
        public GameObject PassUI;
        private GameObject[] m_walls;
        GameModel mGameModel;
   
    
        private void Awake()
        {

            mGameModel=this.GetModel<GameModel>();
            this.RegisterEvent<EventGamePassed>(e =>
            {
                Debug.Log("Game Pass! ");
                PassUI.SetActive(true);
                //PlayUI.SetActive(false);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
           
             
        }
       
        public void Replay()
        {
            SceneManager.LoadScene("PlayLocal");
        }
        void OnDestroy()
        {
          Debug.Log("Game OnDestroy! ");
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
