using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public static Load instance;
    [SerializeField] private Image _load;
    [SerializeField] private float _speed;
    private void Awake()
    {
        if (!instance) instance = this;
    }
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private void LoadedSceneGame()
    {
        AsyncOperation loadSceneGame = SceneManager.LoadSceneAsync("Game");
        _load.fillAmount = loadSceneGame.progress;
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation loadSceneGame = SceneManager.LoadSceneAsync("Game");
        loadSceneGame.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!loadSceneGame.isDone)
        {
            _load.fillAmount = loadSceneGame.progress;
            if (loadSceneGame.progress >= 0.9f)
            {
                loadSceneGame.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
