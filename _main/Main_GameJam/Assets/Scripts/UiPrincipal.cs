using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    public int numberOfChoiceInMenu = 2;
    public int choiceOn = 0;
    public Main main;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI endText;
    public GameObject menuPanel;
    public GameObject gameUi;
    private float countDownDefaultSize;
    [HideInInspector]public float timeCountDownBegin = -1;

    private void Start()
    {
        countDownDefaultSize = startText.rectTransform.rect.height;
        SoundManager.Instance.AddAudioSource(gameObject, GetComponent<AudioSource>());
        SoundManager.Instance.PlayLoop(gameObject, 0);
    }

    public void Refresh()
    {
        if (timeCountDownBegin == -1)
        {
            
        }
        else
            CountDown();
        
    }
    public void CountDown()
    {
        if (timeCountDownBegin >= 0)
        {
            timeCountDownBegin -= Time.deltaTime;
            if (((int) timeCountDownBegin + 1).ToString() != startText.text)
            {
                main.StartShake(1.3f, 0.2f, 0.15f);
                startText.text = ((int) timeCountDownBegin + 1).ToString();
                startText.rectTransform.sizeDelta = new Vector2(startText.rectTransform.rect.width, countDownDefaultSize);
            }
            SoundManager.Instance.StopSound(gameObject);
            Shrink(5);
        }
        else if (timeCountDownBegin > -1)
        {
            timeCountDownBegin -= Time.deltaTime;
            if (startText.text != "START")
            {
                startText.text = "START";
                startText.rectTransform.sizeDelta = new Vector2(startText.rectTransform.rect.width, countDownDefaultSize);
                main.StartShake(2f, 0.2f, 0.5f);
                SoundManager.Instance.PlayLoop(gameObject, 1);
            }
            Shrink(7);
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
        Game.Instance.gameState = Game.GameState.Start;
        StartGame();
        
        
    }

    public void StartMenu()
    {
        menuPanel.SetActive(true);
        gameUi.SetActive(false);
    }
    public void StartGame()
    {
        endText.transform.parent.gameObject.SetActive(false);
        startText.transform.parent.gameObject.SetActive(true);
        timeCountDownBegin = 3;
    }

    public void gameUiActivator()
    {
        menuPanel.SetActive(false);
        gameUi.SetActive(true);
    }
    private void Shrink(int sizeToReduce)
    {
        startText.rectTransform.sizeDelta = new Vector2(startText.rectTransform.rect.width, startText.rectTransform.rect.height - sizeToReduce);
    }

    public void ChangeTextColor()
    {
        
    }
}
