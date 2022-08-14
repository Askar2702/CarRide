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
        // loadSceneGame.allowSceneActivation = false;

        // while (_load.fillAmount < 1)
        // {
        //     // _load.fillAmount = Mathf.MoveTowards(_load.fillAmount, 1f, Time.deltaTime * _speed);
        //     yield return new WaitForSeconds(0.01f);
        // }

        // yield return new WaitUntil(() => _load.fillAmount == 1);
        // loadSceneGame.allowSceneActivation = true;
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation loadSceneGame = SceneManager.LoadSceneAsync("Game");
        loadSceneGame.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        float timer = 0;
        while (!loadSceneGame.isDone)
        {
            timer = loadSceneGame.progress;
            _load.fillAmount = Mathf.Clamp(timer, 0f, 0.7f);
            if (loadSceneGame.progress >= 0.9f)
            {
                loadSceneGame.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
