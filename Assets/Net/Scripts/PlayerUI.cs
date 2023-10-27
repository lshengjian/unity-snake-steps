using UnityEngine;
using UnityEngine.UI;

namespace Mirror.MyGame
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("Player Components")]
        public Image image;

        [Header("Child Text Objects")]
        public Text nameText;
        public Text scoreText;

        // Sets a highlight color for the local player
        public void SetLocalPlayer()
        {
            // add a visual background for the local player in the UI
            image.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // This value can change as clients leave and join
        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            nameText.text = string.Format("Player {0:00}", newPlayerNumber);
        }

        // Random color set by Player::OnStartServer
        public void OnPlayerColorChanged(Color32 newPlayerColor)
        {
            nameText.color = newPlayerColor;
        }

        // This updates from Player::UpdateData via InvokeRepeating on server
        public void OnPlayerScoreChanged(ushort score)
        {
            // Show the data in the UI
            scoreText.text = string.Format("Score: {0:000}", score);
        }
    }
}
