using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour
{
    
    #region Singleton MonoBehaviour
    private static Main instance;

    public static Main Instance { get { return instance; } }
    #endregion

    [Header("Settings")]
    //Variables that can be modified in Inspector
    public int intNotUsed1;
    public string startSceneName = "Scene1";

    [Header("Internal")]
    //Variables that should not be modified from the inspector,
    //but can be used for debugging
    public int intNotUsed2;
    
    private IFlow currentFlow;
    private Game game;

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

        //Setters:
        currentFlow = game;
        
        intNotUsed1 = 0;
        intNotUsed2 = 0;
        
        SceneController.SetCurrentScene(startSceneName);
        
        //Init:
        currentFlow.PreInitialize();
    }

    public void Start()
    {
        currentFlow.Initialize();
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
}
