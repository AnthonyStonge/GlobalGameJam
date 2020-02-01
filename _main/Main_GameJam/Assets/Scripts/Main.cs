using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class Main : MonoBehaviour
{
    
    #region Singleton MonoBehaviour
    private static Main instance;

    public static Main Instance { get { return instance; } }
    #endregion

    public enum FlowState
    {
        Menu,
        Game
    }

    public FlowState flowState = FlowState.Menu;
    //[Header("Settings")]
    //Variables that can be modified in Inspector

    //[Header("Internal")]
    //Variables that should not be modified from the inspector,
    //but can be used for debugging
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private IFlow currentFlow;
    private Game game;
    private Menu menu;
    
    private void Awake()
    {
        
        #region MonoSingleton
        if (Instance != null)
        {
            Destroy(this.gameObject);
            //throw new System.Exception("An instance of this singleton already exists.");
            //On peut aussi faire un return ici
            //return;
        }
        else
        {
            instance = Main.Instance;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
        
        //Instances:
        game = Game.Instance;
        menu = Menu.Instance;

        //Setters:
        
        //Init:
        menu.PreInitialize();
        game.PreInitialize();
    }

    public void Start()
    {
        ChangeFlow("menu");
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Update()
    {
        currentFlow.Refresh();
    }

    public void FixedUpdate()
    {
        currentFlow.PhysicsRefresh();
    }

    public void LateUpdate()
    {
        currentFlow.LateRefresh();
    }

    public void ChangeFlow(string state)
    {
        if(state == "menu")
            currentFlow = menu;
        else
        {
            currentFlow = game;
        }
        currentFlow.Initialize();
    }

    public void StartShake(float ShakeAmplitude, float ShakeFrequency, float ShakeDuration)
    {
        StartCoroutine(shake(ShakeAmplitude, ShakeFrequency, ShakeDuration));
    }
    private IEnumerator shake(float ShakeAmplitude2, float ShakeFrequency2,float ShakeDuration2)
    {
        while (ShakeDuration2 > 0f)
        {
            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude2;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency2;
            ShakeDuration2 -= Time.deltaTime;
            yield return null;
        }
        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
