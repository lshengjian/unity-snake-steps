using System;
using Mirror;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
namespace Mirror.MyGame
{
    public class SnakeNetworkManager : NetworkManager
    {
        [SerializeField] GameObject foodSpawnerPrefab, gameOverHandlerPrefab;
        List<PlayerName> players = new List<PlayerName>();
        [SerializeField] private TMP_Text countdownText;

        public float timeLeft = 90f;
        public bool timerOn = false;

        public override void OnStartServer()
        {
            timerOn = true;
            var gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            // if (numPlayers != 2) return;
            var foodSpawnerInstance = Instantiate(foodSpawnerPrefab);
            NetworkServer.Spawn(foodSpawnerInstance);
        }
        public void FixedUpdate()
        {
            if (numPlayers != 1 && timeLeft > 0f)
            {
                MatchTimer();
            }
            else if (numPlayers == 1 && (timeLeft > 0f || timeLeft == 0f))
            {
                countdownText.transform.parent.gameObject.SetActive(false);
            }
        }

        public void MatchTimer()
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString() + "s";
            if (timerOn)
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                }
                else
                {
                    timeLeft = 0;
                    timerOn = false;
                }
            }
        }
    }
}