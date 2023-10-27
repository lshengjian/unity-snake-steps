using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Mirror.MyGame
{
    public class Player : NetworkBehaviour
    {
        SpriteRenderer mRenderer;
        public event System.Action<byte> OnPlayerIndexChanged;
        public event System.Action<Color32> OnPlayerColorChanged;
        public event System.Action<ushort> OnPlayerScoreChanged;

        // Players List to manage playerNumber
        static readonly List<Player> playersList = new List<Player>();

        [Header("Player UI")]
        public GameObject playerUIPrefab;

        GameObject playerUIObject;
        PlayerUI playerUI = null;

        #region SyncVars

        [Header("SyncVars")]

        /// <summary>
        /// This is appended to the player name text, e.g. "Player 01"
        /// </summary>
        [SyncVar(hook = nameof(PlayerIndexChanged))]
        public byte index = 0;

        /// <summary>
        /// Random color for the playerData text, assigned in OnStartServer
        /// </summary>
        [SyncVar(hook = nameof(PlayerColorChanged))]
        public Color32 color = Color.white;

        /// <summary>
        /// This is updated by UpdateData which is called from OnStartServer via InvokeRepeating
        /// </summary>
        [SyncVar(hook = nameof(PlayerScoreChanged))]
        public ushort score = 0;

        // This is called by the hook of playerNumber SyncVar above
        void PlayerIndexChanged(byte _, byte newPlayerNumber)
        {
            OnPlayerIndexChanged?.Invoke(newPlayerNumber);
        }

        // This is called by the hook of playerColor SyncVar above
        void PlayerColorChanged(Color32 _, Color32 newPlayerColor)
        {
            OnPlayerColorChanged?.Invoke(newPlayerColor);
        }

        // This is called by the hook of playerData SyncVar above
        void PlayerScoreChanged(ushort _, ushort newPlayerData)
        {
            OnPlayerScoreChanged?.Invoke(newPlayerData);
        }

        #endregion

        #region Server

        /// <summary>
        /// This is invoked for NetworkBehaviour objects when they become active on the server.
        /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
        /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();

            // Add this to the static Players List
            playersList.Add(this);

            // set the Player Color SyncVar
            color = Random.ColorHSV(0f, 1f, 0.9f, 0.9f, 1f, 1f);

            // set the initial player data
            score = 0;//(ushort)Random.Range(100, 1000);

            transform.localPosition = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            Food.ServerOnFoodEaten += (obj =>
            {
                if (obj != this.gameObject)
                    return;
                score += 10;

            });//todo


            // Start generating updates
            //  InvokeRepeating(nameof(UpdateData), 1, 1);
            // NetworkServer.Spawn(gameObject);
        }

        // This is called from BasicNetManager OnServerAddPlayer and OnServerDisconnect
        // Player numbers are reset whenever a player joins / leaves
        [ServerCallback]
        internal static void ResetPlayerNumbers()
        {
            byte playerNumber = 0;
            foreach (Player player in playersList)
                player.index = playerNumber++;
        }

        // This only runs on the server, called from OnStartServer via InvokeRepeating
        // [ServerCallback]
        // void UpdateData()
        // {
        //     score = (ushort)Random.Range(0, 1000);
        // }

        /// <summary>
        /// Invoked on the server when the object is unspawned
        /// <para>Useful for saving object data in persistent storage</para>
        /// </summary>
        public override void OnStopServer()
        {
            CancelInvoke();
            playersList.Remove(this);
        }

        #endregion

        #region Client

        /// <summary>
        /// Called on every NetworkBehaviour when it is activated on a client.
        /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
        /// </summary>
        public override void OnStartClient()
        {
            mRenderer = GetComponent<SpriteRenderer>();
            mRenderer.color = color;
            // Instantiate the player UI as child of the Players Panel
            playerUIObject = Instantiate(playerUIPrefab, CanvasUI.GetPlayersPanel());
            playerUI = playerUIObject.GetComponent<PlayerUI>();

            // wire up all events to handlers in PlayerUI
            OnPlayerIndexChanged = playerUI.OnPlayerNumberChanged;
            OnPlayerColorChanged = playerUI.OnPlayerColorChanged;
            OnPlayerScoreChanged = playerUI.OnPlayerScoreChanged;

            // Invoke all event handlers with the initial data from spawn payload
            OnPlayerIndexChanged.Invoke(index);
            OnPlayerColorChanged.Invoke(color);
            OnPlayerScoreChanged.Invoke(score);

        }


        void Update()
        {
            if (!Application.isFocused) return;

            // movement for local player
            if (isLocalPlayer)
            {

                float dx = Input.GetAxis("Horizontal");
                float dy = Input.GetAxis("Vertical");
                Vector3 dv = Vector3.right * dx + Vector3.up * dy;

                transform.Translate(dv * 5f * Time.deltaTime);
            }

        }

        /// <summary>
        /// Called when the local player object has been set up.
        /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            // Set isLocalPlayer for this Player in UI for background shading
            playerUI.SetLocalPlayer();

            // Activate the main panel
            CanvasUI.SetActive(true);
        }

        /// <summary>
        /// Called when the local player object is being stopped.
        /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public override void OnStopLocalPlayer()
        {
            // Disable the main panel for local player
            CanvasUI.SetActive(false);
        }

        /// <summary>
        /// This is invoked on clients when the server has caused this object to be destroyed.
        /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
        /// </summary>
        public override void OnStopClient()
        {
            // disconnect event handlers
            OnPlayerIndexChanged = null;
            OnPlayerColorChanged = null;
            OnPlayerScoreChanged = null;

            // Remove this player's UI object
            Destroy(playerUIObject);
        }

        #endregion
    }
}
