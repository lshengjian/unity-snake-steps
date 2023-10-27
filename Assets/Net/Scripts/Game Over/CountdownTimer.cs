using UnityEngine;
using TMPro;
namespace Mirror.MyGame
{

public class CountdownTimer : NetworkBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    public float timeLeft = 3;
    public bool timerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = Mathf.Ceil(timeLeft).ToString();
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
