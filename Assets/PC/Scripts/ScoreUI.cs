using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    TextMeshProUGUI score_text;

    public Snake player;
    //     private int m_score = 0;
    void Start()
    {
        score_text = GetComponent<TextMeshProUGUI>();
        score_text.color = player.config.color;
        player.score.OnValueChanged+=UpdateText;

    }

    private void UpdateText(int s)
    {


        if (score_text == null)
            return;
        score_text.text = s.ToString();
    }
}
