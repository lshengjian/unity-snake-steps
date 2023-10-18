using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMgr : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene("PlayLocal");
    }

    public void MultiPlayer()
    {
       // SceneManager.LoadScene("MultiPlayer");
    }
}
