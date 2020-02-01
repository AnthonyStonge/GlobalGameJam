using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI startText;
    public TextMeshProUGUI endText;
    [HideInInspector]public float timeCountDownBegin = -1;
    
    
    public void CountDown()
    {
        if (timeCountDownBegin >= 0)
        {
            timeCountDownBegin -= Time.deltaTime;
            if(((int)timeCountDownBegin + 1).ToString() != startText.text)
                startText.text = ((int)timeCountDownBegin + 1).ToString();
        }
        else if (timeCountDownBegin > -1)
        {
            timeCountDownBegin -= Time.deltaTime;
            startText.text = "START";
        }
        else
        {
            if (startText.text != "")
            {
                timeCountDownBegin = -1;
                startText.text = "";
                Game.Instance.gameState = Game.GameState.InGame;
                startText.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void EndGame(bool playerOneWin)
    {
        endText.transform.parent.gameObject.SetActive(true);
    }

    public void Recommencer()
    {
        endText.transform.parent.gameObject.SetActive(false);
        startText.transform.parent.gameObject.SetActive(true);
        Game.Instance.gameState = Game.GameState.Start;
        
        UiManager.Instance.SetBeginGame();
        
    }
}
