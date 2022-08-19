using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class Gift : MonoBehaviour
{
    [SerializeField] private Image _gift;
    [SerializeField] private Image _giftCar;
    private float _time = 1f;
    private Vector2 _startPos;

    private void Start()
    {
        _gift.fillAmount = Game.instance.GiftProgress;
        _startPos = new Vector2(0, -2000);
    }
    public async void ShowGift(Sprite car)
    {
        if (Game.instance.CountOpenCar >= Game.instance.maxCarCount) return;
        _giftCar.sprite = car;
        await _gift.transform.parent.parent.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, _time).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        await _gift.DOFillAmount(_gift.fillAmount + 0.25f, _time).AsyncWaitForCompletion();
        if (_gift.fillAmount >= 1f)
        {
            _giftCar.gameObject.SetActive(true);
            _gift.fillAmount = 0;
            _gift.transform.parent.gameObject.SetActive(false);
            _giftCar.transform.DOScale(new Vector3(1f, 1f, 1f), _time).SetEase(Ease.OutElastic);
            Game.instance.CountOpenCar++;
            await Task.Delay(2000);
        }
        _gift.transform.parent.parent.GetComponent<RectTransform>().DOAnchorPos(_startPos, _time);
        Game.instance.GiftProgress = _gift.fillAmount;
    }
}
