using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class Gift : MonoBehaviour
{
    [SerializeField] private Image _gift;
    [SerializeField] private Image _giftBackground;
    [SerializeField] private Image _giftCar;
    private float _time = 0.5f;
    private Vector2 _startPos;

    private void Start()
    {
        _gift.fillAmount = Game.instance.GiftProgress;
        _startPos = new Vector2(0, -2000);
    }
    public async void ShowGift(Sprite car, Sprite carWhite)
    {
        if (Game.instance.CountOpenCar >= Game.instance.maxCarCount) return;
        if (_giftCar && _gift)
        {
            _giftCar.sprite = car;
            _gift.sprite = carWhite;
            _giftBackground.sprite = carWhite;

            Game.instance.GiftProgress += 0.25f;
            await _gift.transform.parent.parent.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, _time).SetEase(Ease.OutBack).SetLink(_gift.gameObject).AsyncWaitForCompletion();
        }
        if (_gift)
            await _gift.DOFillAmount(_gift.fillAmount + 0.25f, 2f).SetLink(_gift.gameObject).AsyncWaitForCompletion();
        if (_giftCar && _gift && _gift.fillAmount >= 1f)
        {
            Game.instance.GiftProgress = 0;
            _giftCar.gameObject.SetActive(true);
            _gift.fillAmount = 0;
            _gift.transform.parent.gameObject.SetActive(false);
            _giftCar.transform.parent.gameObject.SetActive(true);
            _giftCar.transform.DOScale(new Vector3(1f, 1f, 1f), _time).SetEase(Ease.OutElastic);
            Game.instance.CountOpenCar++;
            await Task.Delay(2000);
        }
        if (_gift)
            _gift.transform.parent.parent.GetComponent<RectTransform>().DOAnchorPos(_startPos, _time).SetLink(_gift.gameObject);

    }
}
