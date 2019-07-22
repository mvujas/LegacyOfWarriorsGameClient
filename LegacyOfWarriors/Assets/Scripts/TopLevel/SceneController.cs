using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region EDITOR FIELDS
    [SerializeField]
    private string[] sceneNames = { };
    [SerializeField]
    private string startingScene = null;

    private bool DoesContainScene(string sceneName)
    {
        return sceneNames != null && sceneNames.Contains(sceneName);
    }

    private void OnValidate()
    {
        if (!DoesContainScene(startingScene) || startingScene == "")
        {
            startingScene = null;
        }
    }
    #endregion

    private string m_activeSceneName = null;
    private object m_sceneChangingLock = new object();

    public void LoadScene(string sceneName)
    {
        if(sceneName == null)
        {
            return;
        }
        if(!DoesContainScene(sceneName))
        {
            throw new System.Exception("Can't load scene that doesn't exist in SceneController");
        }
        lock(m_sceneChangingLock)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (m_activeSceneName != null)
            {
                SceneManager.UnloadSceneAsync(m_activeSceneName);
            }
            m_activeSceneName = sceneName;
        }
    }

    private void Awake()
    {
        sceneNames = sceneNames.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        LoadScene(startingScene);
    }
}
