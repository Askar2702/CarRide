using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;


public class FinishManager : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Transform[] _stars;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private float _time;
    [SerializeField] private Ease _ease;
    [SerializeField] private Image _gift;
    [SerializeField] private Image _giftCar;
    private SpawnCoin _spawnCoin;
    private void Start()
    {
        _spawnCoin = FindObjectOfType<SpawnCoin>();
        _gift.fillAmount = _game.GiftProgress;
        GameManager.instance.Finishing.AddListener(CheckProgressPlayer);
    }

    private async void CheckProgressPlayer()
    {
        await Task.Delay(1000);
        var count = CheckCoins();
        for (int i = 0; i < count; i++)
        {
            await _stars[i].DOScale(new Vector3(1f, 1f, 1f), _time).SetEase(_ease).AsyncWaitForCompletion();
        }
        await _gift.DOFillAmount(_gift.fillAmount + 0.25f, _time).AsyncWaitForCompletion();
        if (_gift.fillAmount >= 1f)
        {
            _giftCar.gameObject.SetActive(true);
            _gift.fillAmount = 0;
            _gift.transform.parent.gameObject.SetActive(false);
            _giftCar.transform.DOScale(new Vector3(1f, 1f, 1f), _time).SetEase(_ease);
        }
        _game.GiftProgress = _gift.fillAmount;
    }

    private int CheckCoins()
    {
        int count = 0;
        _coins.text = UiManager.instance.CoinAmount.ToString();
        if (UiManager.instance.CoinAmount == 0) return 0;
        if (_spawnCoin.CountCoins == UiManager.instance.CoinAmount) count = _stars.Length;
        else if ((UiManager.instance.CoinAmount) >= (_spawnCoin.CountCoins / 2)) count = _stars.Length - 1;
        else if ((UiManager.instance.CoinAmount) < (_spawnCoin.CountCoins / 2)) count = _stars.Length - 2;
        return count;
    }
}
