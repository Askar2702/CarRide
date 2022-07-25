using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public event Action<ButtonState> Act;
    #region UI
    [SerializeField] private RectTransform _panel;
    [SerializeField] private RectTransform _UpPos;
    [SerializeField] private RectTransform _centerPos;
    [SerializeField] private GameObject _iconPause;
    [SerializeField] private GameObject _iconPlay;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private int _coinAmount;
    #endregion
    #region UI Button
    [SerializeField] private Button _start;
    [SerializeField] private Button _exit;
    [SerializeField] private Button _nextGame;
    [SerializeField] private Button _restart;
    [SerializeField] private Button _pause;
    #endregion

    [SerializeField] private float _speed;
    [SerializeField] private Ease _ease;

    [SerializeField] private Animation _anim;
    private void Awake()
    {
        if (!instance) instance = this;
        _start.onClick.AddListener(StartGame);
        _nextGame.onClick.AddListener(() => NextGame());
        _exit.onClick.AddListener(() => ExitGame());
        _restart.onClick.AddListener(() => RestartGame());
        _pause.onClick.AddListener(() => PauseGame());
    }

    private void Start()
    {
        GameManager.instance.Finishing += ShowPanel;
    }
    private void StartGame()
    {
        Act?.Invoke(ButtonState.Start);
        SetPosPanel(_UpPos);
    }
    private void NextGame()
    {
        Act?.Invoke(ButtonState.Next);
    }
    private void RestartGame()
    {
        Act?.Invoke(ButtonState.Restart);
    }
    private void ExitGame()
    {
        Act?.Invoke(ButtonState.Exit);
    }

    private void PauseGame()
    {
        if (GameManager.instance.GameState != GameState.Play) return;
        if (_panel.anchoredPosition == _UpPos.anchoredPosition)
        {
            SetPosPanel(_centerPos);
            _start.gameObject.SetActive(false);
            _nextGame.gameObject.SetActive(false);
            _restart.gameObject.SetActive(true);
            _iconPause.SetActive(false);
            _iconPlay.SetActive(true);
            _exit.gameObject.SetActive(true);
        }
        else if (_panel.anchoredPosition != _UpPos.anchoredPosition)
        {
            SetPosPanel(_UpPos);
            _iconPause.SetActive(true);
            _iconPlay.SetActive(false);
        }
    }

    public void ShowPanel()
    {
        SetPosPanel(_centerPos);
        _start.gameObject.SetActive(false);
        _nextGame.gameObject.SetActive(true);
        _restart.gameObject.SetActive(false);
        _exit.gameObject.SetActive(true);
    }

    private void SetPosPanel(RectTransform pos)
    {
        _panel.DOAnchorPos(pos.anchoredPosition, _speed).SetEase(_ease);
    }


    public void CoinAdd()
    {
        _coinAmount++;
        _coinText.text = _coinAmount.ToString();
        _anim.Play();
        // _coinText.transform.DOPunchScale(Vector3.up, 0.2f, 1, 0.2f);
    }
}
