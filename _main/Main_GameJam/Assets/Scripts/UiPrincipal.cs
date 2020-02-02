using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    public int numberOfChoiceInMenu = 2;
    public int selectedChoice = 0;
    public Main main;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI playerWinText;
    public TextMeshProUGUI endText;
    public GameObject menuPanel;
    public GameObject gameUi;
    public GameObject instruction;
    [SerializeField] private List<TextMeshProUGUI> choiceMenu;
    private float countDownDefaultSize;
    [HideInInspector]public float timeCountDownBegin = -1;

    private void Start()
    {
        countDownDefaultSize = startText.rectTransform.rect.height;
        SoundManager.Instance.AddAudioSource(gameObject, GetComponent<AudioSource>());
        SoundManager.Instance.PlayLoop(gameObject, 0);
    }

    public void Update()
    {
        if (timeCountDownBegin != -1)
        {
            //CountDown();
        }

        if (main.flowState == Main.FlowState.Menu)
        {
            for (int i = 0; i < choiceMenu.Count; i++)
            {
                /*if (i == selectedChoice)
                {
                    choiceMenu[selectedChoice].GetComponent<UiChangeColor>().ChangeColorEnter();
                }
                else
                {
                    choiceMenu[selectedChoice].GetComponent<UiChangeColor>().ChangeColorExit();
                }*/
            }
            
            
        }

    }
    public void CountDown()
    {
        if (timeCountDownBegin >= 0)
        {
            timeCountDownBegin -= Time.deltaTime;
            if (((int) timeCountDownBegin + 1).ToString() != startText.text)
            {
                UiManager.Instance.SetCamFov();
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
                //startText.fontSize -= 10;
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
        if(playerOneWin)
            playerWinText.text = "Player 1 Win";
        else
        {
            playerWinText.text = "Player 2 Win";
        }
    }

    public void Recommencer()
    {
        Game.Instance.gameState = Game.GameState.Start;
        PlayerManager.Instance.ReinitializePosition();
        
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

    public void ShowInstruvtion()
    {
        menuPanel.SetActive(false);
        instruction.SetActive(true);
    }
    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        instruction.SetActive(false);
    }
}
