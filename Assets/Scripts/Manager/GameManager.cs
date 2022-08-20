using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
[RequireComponent(typeof(UiManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UnityEvent Finishing;
    [SerializeField] private GenerationMap _generator;
    [SerializeField] private Game _game;
    [field: SerializeField] public Transform FinishPos;

    public GameState GameState { get; private set; }
    [field: SerializeField] public Transform UICoinPos;
    [SerializeField] private ParticleSystem _conffetti;
    [SerializeField] private AudioClip _audioFinish;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RoadCurveSetting _roadCurveSetting;
    private UiManager _uiManager;
    private void Awake()
    {
        if (!instance) instance = this;
        _uiManager = GetComponent<UiManager>();
    }
    void Start()
    {
        SetstateGame(GameState.Start);
        _uiManager.Act += ButtonAction;
    }

    public void Finish()
    {
        if (GameState == GameState.Finish) return;
        Finishing?.Invoke();
        SetstateGame(GameState.Finish);
        _conffetti.Play();
        _audioSource.PlayOneShot(_audioFinish);
    }

    public void Lose()
    {
        SaveData(false);
        SceneManager.LoadScene("LoadScene");
    }


    private void PauseGame()
    {
        if (GameState == GameState.Stop) GameState = GameState.Play;
        else GameState = GameState.Stop;
    }
    private void ButtonAction(ButtonState buttonState)
    {
        if (buttonState == ButtonState.Start) SetstateGame(GameState.Play);
        else if (buttonState == ButtonState.Restart) Lose();
        else if (buttonState == ButtonState.Next) SceneManager.LoadScene("LoadScene");
        else if (buttonState == ButtonState.Exit) Application.Quit();
        else if (buttonState == ButtonState.Pause) PauseGame();
    }

    private void SetstateGame(GameState gameState)
    {
        GameState = gameState;
    }

    public void SaveData(bool activ)
    {
        _game.Seed = _generator.Seed;
        _game.IsRandomGeneration = activ;
        _game.SetRoadOffset(_generator.Offset);
        if (activ)
        {
            Game.instance.Level++;
            Game.instance.IndexRoad++;
            if (Game.instance.IndexRoad >= _roadCurveSetting.RoadSettings.Length) Game.instance.IndexRoad = 0;
        }
    }
    public (bool, int, Vector2) LoadData()
    {
        return (_game.IsRandomGeneration, _game.Seed, _game.RoadOffset);
    }
}
