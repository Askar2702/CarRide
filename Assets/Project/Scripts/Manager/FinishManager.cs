using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;


public class FinishManager : MonoBehaviour
{
    [SerializeField] private Transform[] _stars;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private float _speed;
    [SerializeField] private Ease _ease;
    [SerializeField] private Sprite[] _carImages;
    [SerializeField] private Sprite[] _carWhiteImages;
    [SerializeField] private Gift _gift;
    private SpawnCoin _spawnCoin;

    #region Timer
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _finishTimer;
    private int _minute;
    private int _secund;
    #endregion
    private void Start()
    {
        _spawnCoin = FindObjectOfType<SpawnCoin>();
        GameManager.instance.Finishing.AddListener(CheckProgressPlayer);
        _coins.text = $"{_spawnCoin.CountCoins} / {UiManager.instance.CoinAmount.ToString()}";
        StartCoroutine(TimerGame());
    }

    private async void CheckProgressPlayer()
    {
        await Task.Delay(1000);
        var count = CheckCoins();
        for (int i = 0; i < count; i++)
        {
            await _stars[i].DOScale(new Vector3(1f, 1f, 1f), _speed).SetEase(_ease).AsyncWaitForCompletion();
        }
        if (Game.instance.CountOpenCar < _carImages.Length)
            _gift.ShowGift(_carImages[Game.instance.CountOpenCar], _carWhiteImages[Game.instance.CountOpenCar]);
    }

    private int CheckCoins()
    {
        int count = 0;
        _coins.text = $"{_spawnCoin.CountCoins} / {UiManager.instance.CoinAmount.ToString()}";
        if (UiManager.instance.CoinAmount == 0) return 0;
        if (_spawnCoin.CountCoins == UiManager.instance.CoinAmount) count = _stars.Length;
        else if ((UiManager.instance.CoinAmount) >= (_spawnCoin.CountCoins / 2)) count = _stars.Length - 1;
        else if ((UiManager.instance.CoinAmount) < (_spawnCoin.CountCoins / 2)) count = _stars.Length - 2;
        return count;
    }

    IEnumerator TimerGame()
    {
        var time = 0f;
        bool isStop = false;
        while (!isStop)
        {
            if (GameManager.instance.GameState == GameState.Play)
            {
                time += Time.deltaTime;
                _secund = (int)(time % 60f);
                _minute = (int)(time / 60 % 60f);
                _timer.text = string.Format("{0:00}:{1:00}", _minute, _secund);
            }
            if (GameManager.instance.GameState == GameState.Finish)
            {
                _finishTimer.text = string.Format("{0:00}:{1:00}", _minute, _secund);
                isStop = true;
            }
            yield return null;
        }
    }
}
