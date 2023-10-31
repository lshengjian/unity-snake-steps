using UnityEngine;

namespace Mirror.MyGame
{
    public class CanvasUI : MonoBehaviour
    {
        [Tooltip("Assign Main Panel so it can be turned on from Player:OnStartClient")]
        public RectTransform mainPanel;

        [Tooltip("Assign Players Panel for instantiating PlayerUI as child")]
        public RectTransform playersPanel;
 public RectTransform overPanel;
        // static instance that can be referenced from static methods below.
        static CanvasUI instance;

        void Awake()
        {
            instance = this;
        }

        public static void SetActive(bool active)
        {
            instance.mainPanel.gameObject.SetActive(active);
            instance.overPanel.gameObject.SetActive(!active);
           //  Time.timeScale=active?1f:0f;
        }

        public static RectTransform GetPlayersPanel() => instance.playersPanel;
    }
}
