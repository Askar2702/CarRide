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
    [SerializeField] private RectTransform _panelStartAndPause;
    [SerializeField] private RectTransform _panelFinish;
    [SerializeField] private RectTransform _UpPos;
    [SerializeField] private RectTransform _centerPos;
    [SerializeField] private GameObject _iconPause;
    [SerializeField] private GameObject _iconPlay;
    [SerializeField] private GameObject _iconSoundPause;
    [SerializeField] private GameObject _iconSoundPlay;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI[] _gemTexts;
    [SerializeField] private TextMeshProUGUI _levelLabel;
    public int CoinAmount { get; private set; }
    public int GemAmount { get; private set; }

    #endregion
    #region UI Button
    [SerializeField] private Button _start;
    [SerializeField] private Button _exit;
    [SerializeField] private Button _nextGame;
    [SerializeField] private Button[] _restart;
    [SerializeField] private Button _pause;
    [SerializeField] private Button _setSound;
    [SerializeField] private Button _machineControlsetting;
    #endregion

    #region Other
    [SerializeField] private float _speed;
    [SerializeField] private Ease _ease;

    [SerializeField] private Animation[] _anim;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _gemSound;
    [SerializeField] private GameObject _machineControlImgYes;
    [SerializeField] private GameObject _machineControlImgNo;
    [SerializeField] private GameObject _steeringWheel;
    [SerializeField] private GameObject _buttonsArrowParent;
    private const int MAXLEVEL = 99;
    #endregion
    private Tween _sequence;
    private int _currentCountGem;

    private void Awake()
    {
        if (!instance) instance = this;
        _start.onClick.AddListener(StartGame);
        _nextGame.onClick.AddListener(() => NextGame());
        _exit.onClick.AddListener(() => ExitGame());
        _machineControlsetting.onClick.AddListener(() => MachineControlSetting());
        foreach (var item in _restart)
        {
            item.onClick.AddListener(() => RestartGame());
        }

        _pause.onClick.AddListener(() => PauseGame());
        _setSound.onClick.AddListener(() => EnableSound());
        DOTween.Init();
    }


    private void Start()
    {
        GameManager.instance.Finishing.AddListener(ShowFinishPanel);
        LoadSetting();
        _currentCountGem = 0;
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
        Time.timeScale = 1;
    }
    private void ExitGame()
    {
        Time.timeScale = 1;
        Act?.Invoke(ButtonState.Exit);
    }

    private void PauseGame()
    {
        if (GameManager.instance.GameState != GameState.Play) return;
        if (_panelStartAndPause.anchoredPosition == _UpPos.anchoredPosition)
        {
            SetPosPanel(_centerPos);
            _start.gameObject.SetActive(false);
            _restart[0].gameObject.SetActive(true);
            _iconPause.SetActive(false);
            _iconPlay.SetActive(true);
            _exit.gameObject.SetActive(true);
            _setSound.gameObject.SetActive(true);
            _machineControlsetting.gameObject.SetActive(true);
            Time.timeScale = 0;
            CarManager.instance.CurrentCar.SetEnableAudio(false);
        }
        else if (_panelStartAndPause.anchoredPosition != _UpPos.anchoredPosition)
        {
            SetPosPanel(_UpPos);
            _iconPause.SetActive(true);
            _iconPlay.SetActive(false);
            Time.timeScale = 1;
            CarManager.instance.CurrentCar.SetEnableAudio(true);
        }
    }

    private void EnableSound()
    {
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
            Game.instance.IsSound = false;
            _iconSoundPause.SetActive(false);
            _iconSoundPlay.SetActive(true);
        }
        else
        {
            AudioListener.pause = true;
            Game.instance.IsSound = true;
            _iconSoundPause.SetActive(true);
            _iconSoundPlay.SetActive(false);
        }
    }

    private void LoadSetting()
    {
        if (Game.instance.IsSound)
        {
            AudioListener.pause = true;
            _iconSoundPause.SetActive(true);
            _iconSoundPlay.SetActive(false);
        }
        else
        {
            AudioListener.pause = false;
            _iconSoundPause.SetActive(false);
            _iconSoundPlay.SetActive(true);
        }
        LoadInfoGem(Game.instance.CountGem);
        if (Game.instance.Level >= MAXLEVEL) Game.instance.Level = 0;
        _levelLabel.text = $"{Game.instance.Level + 1}";
        TinySauce.OnGameStarted(_levelLabel.text);
        if (Game.instance.SteeringWheel)
        {
            _machineControlImgYes.SetActive(false);
            _machineControlImgNo.SetActive(true);
            _steeringWheel.SetActive(false);
            _buttonsArrowParent.SetActive(true);
        }
        else
        {
            _machineControlImgYes.SetActive(true);
            _machineControlImgNo.SetActive(false);
            _steeringWheel.SetActive(true);
            _buttonsArrowParent.SetActive(false);
        }
    }

    public void ShowFinishPanel()
    {
        _panelFinish.DOAnchorPos(_centerPos.anchoredPosition, _speed).SetEase(_ease).SetUpdate(true);
        TinySauce.OnGameFinished(true, CoinAmount, _levelLabel.text);
    }

    private void SetPosPanel(RectTransform pos)
    {
        if (_sequence == null) _sequence = _panelStartAndPause.DOAnchorPos(pos.anchoredPosition, _speed).SetEase(_ease).SetUpdate(true);
        else
        {
            _sequence.Kill(true);
            _sequence = _panelStartAndPause.DOAnchorPos(pos.anchoredPosition, _speed).SetEase(_ease).SetUpdate(true);
        }
    }


    public void CoinAdd()
    {
        CoinAmount++;
        _coinText.text = CoinAmount.ToString();
        _anim[0].Play();
        _audio.PlayOneShot(_coinSound);
    }

    public void ResetProgress()
    {
        Game.instance.CountGem -= _currentCountGem;
    }
    public void AddGem(int i)
    {
        _currentCountGem++;
        _anim[1].Play();
        _audio.PlayOneShot(_gemSound);
        LoadInfoGem(i);
    }

    private void MachineControlSetting()
    {
        if (Game.instance.SteeringWheel)
        {
            _machineControlImgYes.SetActive(true);
            _machineControlImgNo.SetActive(false);
            _steeringWheel.SetActive(true);
            _buttonsArrowParent.SetActive(false);
            Game.instance.SteeringWheel = false;
        }
        else
        {
            _machineControlImgYes.SetActive(false);
            _machineControlImgNo.SetActive(true);
            _steeringWheel.SetActive(false);
            _buttonsArrowParent.SetActive(true);
            Game.instance.SteeringWheel = true;
        }
    }
    private void LoadInfoGem(int i)
    {
        foreach (var item in _gemTexts)
        {
            item.text = i.ToString();
        }
    }
}
