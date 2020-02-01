using System.Collections;
using System.Collections.Generic;
using PixelCrushers.SceneStreamer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController
{
    public int maxNeighborDistance = 1;
    public float maxLoadWaitTime = 10f;

    [System.Serializable]
    public class StringEvent : UnityEvent<string>
    {
    }

    [System.Serializable]
    public class StringAsyncEvent : UnityEvent<string, AsyncOperation>
    {
    }

    public StringAsyncEvent onLoading = new StringAsyncEvent();
    public StringEvent onLoaded = new StringEvent();

    public bool debug = false;

    public bool logDebugInfo
    {
        get { return debug && Debug.isDebugBuild; }
    }

    private string currentSceneName = null;
    private HashSet<string> loaded = new HashSet<string>();
    private HashSet<string> loading = new HashSet<string>();

    /// The names of all scenes within maxNeighborDistance of the current scene.
    /// This is used when determining which neighboring scenes to load or unload.
    private HashSet<string> near = new HashSet<string>();

    #region Singleton

    private static SceneController instance = null;

    private SceneController()
    {
    }

    public static SceneController Instance => instance ?? (instance = new SceneController());

    #endregion

    private void LoadSceneCoroutine(string sceneName)
    {
        Main.Instance.StartCoroutine(LoadCurrentScene(sceneName));
    }

    private void LoadAdditiveAsyncCoroutine(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        Main.Instance.StartCoroutine(LoadAdditiveAsync(sceneName, loadedHandler, distance));
    }

    /// Sets the current scene, loads it, and manages neighbors. The scene must be in your
    /// project's build settings.
    public void SetCurrent(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, currentSceneName)) return;
        if (logDebugInfo) Debug.Log("Scene Streamer: Setting current scene to " + sceneName + ".");
        LoadSceneCoroutine(sceneName);
    }

    /// Loads a scene as the current scene and manages neighbors, loading scenes
    /// within maxNeighborDistance and unloading scenes beyond it.
    private IEnumerator LoadCurrentScene(string sceneName)
    {
        // First load the current scene:
        currentSceneName = sceneName;
        if (!IsLoaded(currentSceneName)) Load(sceneName);
        float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime))
        {
            yield return null;
        }

        if (Time.realtimeSinceStartup >= failsafeTime && Debug.isDebugBuild)
            Debug.LogWarning("Scene Streamer: Timed out waiting to load " + sceneName + ".");

        // Next load neighbors up to maxNeighborDistance, keeping track
        // of them in the near list:
        if (logDebugInfo)
            Debug.Log("Scene Streamer: Loading " + maxNeighborDistance + " closest neighbors of " + sceneName + ".");
        near.Clear();
        LoadNeighbors(sceneName, 0);
        failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime))
        {
            yield return null;
        }

        if (Time.realtimeSinceStartup >= failsafeTime && Debug.isDebugBuild)
            Debug.LogWarning("Scene Streamer: Timed out waiting to load neighbors of " + sceneName + ".");

        // Finally unload any scenes not in the near list:
        UnloadFarScenes();
    }

    /// Loads neighbor scenes within maxNeighborDistance, adding them to the near list.
    private void LoadNeighbors(string sceneName, int distance)
    {
        if (near.Contains(sceneName)) return;
        near.Add(sceneName);
        if (distance >= maxNeighborDistance) return;
        GameObject scene = GameObject.Find(sceneName);
        NeighboringScenes neighboringScenes = (scene) ? scene.GetComponent<NeighboringScenes>() : null;
        if (!neighboringScenes) neighboringScenes = CreateNeighboringScenesList(scene);
        if (!neighboringScenes) return;
        for (int i = 0; i < neighboringScenes.sceneNames.Length; i++)
        {
            Load(neighboringScenes.sceneNames[i], LoadNeighbors, distance + 1);
        }
    }

    /// Creates the neighboring scenes list. It's faster to manually add a
    /// NeighboringScenes script to your scene's root object; this method
    /// builds it manually if it's missing, but requires the scene to have
    /// SceneEdge components.
    private NeighboringScenes CreateNeighboringScenesList(GameObject scene)
    {
        if (!scene) return null;
        NeighboringScenes neighboringScenes = scene.AddComponent<NeighboringScenes>();
        HashSet<string> neighbors = new HashSet<string>();
        var sceneEdges = scene.GetComponentsInChildren<SceneEdge>();
        for (int i = 0; i < sceneEdges.Length; i++)
        {
            neighbors.Add(sceneEdges[i].nextSceneName);
        }

        neighboringScenes.sceneNames = new string[neighbors.Count];
        neighbors.CopyTo(neighboringScenes.sceneNames);
        return neighboringScenes;
    }

    /// Determines whether a scene is loaded.
    public bool IsLoaded(string sceneName)
    {
        return loaded.Contains(sceneName);
    }

    /// Loads a scene.
    public void Load(string sceneName)
    {
        Load(currentSceneName, null, 0);
    }

    private delegate void InternalLoadedHandler(string sceneName, int distance);

    /// Loads a scene and calls an internal delegate when done. The delegate is
    /// used by the LoadNeighbors() method.
    private void Load(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        if (IsLoaded(sceneName))
        {
            if (loadedHandler != null) loadedHandler(sceneName, distance);
            return;
        }

        loading.Add(sceneName);
        if (logDebugInfo && distance > 0) Debug.Log("Scene Streamer: Loading " + sceneName + ".");
        LoadAdditiveAsyncCoroutine(sceneName, loadedHandler, distance);
    }

    /// (Unity Pro) Runs Application.LoadLevelAdditiveAsync() and calls FinishLoad() when done.
    private IEnumerator LoadAdditiveAsync(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        onLoading.Invoke(sceneName, asyncOperation);
        yield return asyncOperation;
        FinishLoad(sceneName, loadedHandler, distance);
    }

    /// (Unity) Runs Application.LoadLevelAdditive() and calls FinishLoad() when done.
    /// This coroutine waits two frames to wait for the load to complete.
    private IEnumerator LoadAdditive(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        onLoading.Invoke(sceneName, null);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        FinishLoad(sceneName, loadedHandler, distance);
    }

    /// Called when a level is done loading. Updates the loaded and loading lists, and 
    /// calls the loaded handler.
    private void FinishLoad(string sceneName, InternalLoadedHandler loadedHandler, int distance)
    {
        GameObject scene = GameObject.Find(sceneName);
        if (scene == null && Debug.isDebugBuild)
            Debug.LogWarning("Scene Streamer: Can't find loaded scene named '" + sceneName + "'.");
        loading.Remove(sceneName);
        loaded.Add(sceneName);
        onLoaded.Invoke(sceneName);
        if (loadedHandler != null) loadedHandler(sceneName, distance);
    }

    /// Unloads scenes beyond maxNeighborDistance. Assumes the near list has already been populated.
    private void UnloadFarScenes()
    {
        HashSet<string> far = new HashSet<string>(loaded);
        far.ExceptWith(near);
        if (logDebugInfo && far.Count > 0)
            Debug.Log("Scene Streamer: Unloading scenes more than " + maxNeighborDistance +
                      " away from current scene " + currentSceneName + ".");
        foreach (var sceneName in far)
        {
            Unload(sceneName);
        }
    }

    /// Unloads a scene.
    public void Unload(string sceneName)
    {
        if (logDebugInfo) Debug.Log("Scene Streamer: Unloading scene " + sceneName + ".");
        GameObject.Destroy(GameObject.Find(sceneName));
        loaded.Remove(sceneName);
        SceneManager.UnloadSceneAsync(sceneName);
    }

    /// Sets the current scene.
    public static void SetCurrentScene(string sceneName)
    {
        instance.SetCurrent(sceneName);
    }

    /// Determines if a scene is loaded.
    public static bool IsSceneLoaded(string sceneName)
    {
        return instance.IsLoaded(sceneName);
    }

    /// Loads a scene.
    public static void LoadScene(string sceneName)
    {
        instance.Load(sceneName);
    }

    /// Unloads a scene.
    public static void UnloadScene(string sceneName)
    {
        instance.Unload(sceneName);
    }
}