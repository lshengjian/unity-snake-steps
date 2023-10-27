
using TMPro;
using UnityEngine;
namespace Mirror.MyGame
{
public class PlayerName : NetworkBehaviour
{
    [SerializeField] TMP_Text playerNameText;
    [SyncVar(hook = nameof(HandlePlayerNameUpdated))]
    string playerName;

    public string Name { get { return playerName; } }

    void HandlePlayerNameUpdated(string oldText, string newText)
    {
        playerNameText.text = newText;
    }

    public override void OnStartServer()
    {
        playerName = $"Player_{connectionToClient.connectionId}";
    }
}
}