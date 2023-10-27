using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;
using UnityEngine.SceneManagement;
namespace Mirror.MyGame
{
public class PlayerSnake : NetworkBehaviour
{
    [SerializeField] TailSpawner tailSpawner;
    [SerializeField] PlayerName playerName;
    public static event Action<PlayerName> ServerOnPlayerSpawned;
    public static event Action<PlayerName> ServerOnPlayerDespawned;
    
    public int points = 10;
    private int score = 0;
    private bool hasCollided;
    public TMP_Text scoreText;

    public override void OnStartServer()
    {
        ServerOnPlayerSpawned?.Invoke(playerName);
        hasCollided = false;
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out NetworkIdentity networkIdentity)
            && networkIdentity.connectionToClient == connectionToClient)
            return;
        switch (other.tag)
        {
            case "Points":
                if (!hasCollided)
                {
                    hasCollided = true;
                    score += points;
                    Debug.Log(score);
                    scoreText.text = score.ToString();
                }
                hasCollided = false;
                return;
            case "Border":
            case "Player":
            case "Tail":
            case "Danger":
                //StartCoroutine(ReturnToMainMenu());
                DestroySelf();
                break;
        }
    }

    void DestroySelf()
    {
        ServerOnPlayerDespawned?.Invoke(playerName);
        foreach (var tail in tailSpawner.Tails)
            NetworkServer.Destroy(tail);
        NetworkServer.Destroy(gameObject);
    }

    IEnumerator ReturnToMainMenu()
    {
        //DestroySelf();
        yield return new WaitForSeconds(3);
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("MainMenu");
    }
}
}