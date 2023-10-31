using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMgr : MonoBehaviour
{
    public void PlayOnPC()
    {
        SceneManager.LoadScene("PlayLocal");
    }

    public void PlayOnNet()
    {
        SceneManager.LoadScene("BasicNetDemo");
    }
}
