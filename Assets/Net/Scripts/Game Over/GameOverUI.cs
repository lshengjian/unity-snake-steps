
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Mirror.MyGame
{
  public class GameOverUI : MonoBehaviour
  {
 //
    
    void LateUpdate()
    {
   //   if (isOver)
   //   SceneManager.LoadScene("MainMenu");

    }
     public void BackToMenu()
        {
         //  Time.timeScale = 1f;
            Debug.Log("BackToMenu");
            SceneManager.LoadScene("MainMenu");

        }
  }
}