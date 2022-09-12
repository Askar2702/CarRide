using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public UnityEvent ShowIngo;
    [SerializeField] private RectTransform _rightPos;
    [SerializeField] private RectTransform _panelShop;
    [SerializeField] private RectTransform _btnShowShop;
    [SerializeField] private Button _btnClose;
    [SerializeField] private float _speed;
    [SerializeField] private Ease _ease;

    private void Awake()
    {
        if (!instance) instance = this;
        _btnShowShop.GetComponent<Button>().onClick.AddListener(ShowPanel);
        _btnClose.onClick.AddListener(CloseShop);
    }

    private void ShowPanel()
    {
        if (GameManager.instance.GameState == GameState.Finish) return;
        _panelShop.DOAnchorPos(Vector2.zero, _speed).SetEase(_ease).SetUpdate(true);
        _btnShowShop.DOAnchorPos(_rightPos.anchoredPosition, _speed).SetUpdate(true);
        Time.timeScale = 0;
        CarManager.instance.CurrentCar.SetEnableAudio(false);
        ShowIngo?.Invoke();
    }
    private void CloseShop()
    {
        _panelShop.DOAnchorPos(_rightPos.anchoredPosition, _speed).SetEase(_ease);
        _btnShowShop.DOAnchorPos(new Vector2(10f, 0f), _speed);
        Time.timeScale = 1;
        CarManager.instance.CurrentCar.SetEnableAudio(true);
    }
}
