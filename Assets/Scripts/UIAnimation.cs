using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Transform[] _stars;
    [SerializeField] private float _time;
    [SerializeField] private Ease _ease;
    [SerializeField] private Image _gift;
    [SerializeField] private Image _giftCar;

    private void Start()
    {
        _gift.fillAmount = _game.giftProgress;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) CheckProgressPlayer(3);
    }
    public async void CheckProgressPlayer(int count)
    {
        if (count > _stars.Length) count = _stars.Length;
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
        GameManager.instance.giftProgress = _gift.fillAmount;
    }
}
