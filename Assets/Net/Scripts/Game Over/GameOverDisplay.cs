using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Mirror.MyGame
{
public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] GameObject canvas;
   // [SerializeField] DisplayAlert displayAlert;
    [SerializeField] TMP_Text winnerNameText;
    


    void Start()
    {
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    void ClientHandleGameOver(string winner)
    {
        canvas.SetActive(true);
        winnerNameText.text = $"{winner} Wins!";
      //  displayAlert.ShowAlert();
    }

    public void RestartGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else NetworkManager.singleton.StopClient();
        canvas.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
}