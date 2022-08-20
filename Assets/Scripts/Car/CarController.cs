using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{
    public enum StateMove { Acceleration = 0, Zero = 1, Back = 2 }
    private enum StateSteeringWheel { Right = 0, Middle = 1, Left = 2 }

    [field: SerializeField] public int Id { get; private set; }
    private StateMove _stateMove;
    private StateSteeringWheel _stateSteeringWheel;
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    private const float _breakForse = 2000f;
    public float CurrentBreakForse { get; private set; }
    [SerializeField] private Light[] _lights;
    [SerializeField] private CarSound _carSound;
    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private ParticleSystem _windEffect;
    private Rigidbody _rb;
    private float _carSpeedForWind = 25;
    public void SetSteer(float s)
    {
        Horizontal = s;
    }
    private void Start()
    {
        _stateMove = StateMove.Zero;
        _stateSteeringWheel = StateSteeringWheel.Middle;
        CurrentBreakForse = _breakForse;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.instance.GameState == GameState.Play)
        {
            Move();
            if (transform.position.y < -100f) GameManager.instance.Lose();

            if (_rb.velocity.magnitude >= _carSpeedForWind)
            {
                if (_windEffect.isPlaying) return;
                _windEffect.Play();
            }
            else _windEffect.Stop();
        }
        else if (GameManager.instance.GameState == GameState.Finish)
        {
            ApplyBreaking(true);
        }
    }


    private void Move()
    {
        if (_stateMove == StateMove.Acceleration)
        {
            Vertical = Mathf.Clamp(Vertical + Time.fixedDeltaTime, -1, 1);
            _carSound.PlayAccelerationSong();
        }
        else if (_stateMove == StateMove.Back)
        {
            Vertical = Mathf.Clamp(Vertical - Time.fixedDeltaTime, -1, 1);
            _carSound.PlayDeAccelerationSong();
        }
        else
        {
            if (Vertical == 0) return;
            if (Vertical < 0) Vertical = Mathf.Clamp(Vertical + Time.fixedDeltaTime, -1, 1);
            else
            {
                _carSound.PlayDeAccelerationSong();
                Vertical = Mathf.Clamp(Vertical - Time.fixedDeltaTime, -1, 1);
            }
        }
    }
    public void MoveForward(int i)
    {
        ApplyBreaking(false);
        _stateMove = (StateMove)i;
        if (_stateMove == StateMove.Back)
        {
            foreach (var l in _lights) l.gameObject.SetActive(true);
        }
        else
        {
            foreach (var l in _lights) l.gameObject.SetActive(false);
        }
    }
    public void TurnSide(int i)
    {
        _stateSteeringWheel = (StateSteeringWheel)i;
    }

    public void ApplyBreaking(bool activ)
    {
        CurrentBreakForse = activ ? _breakForse : 0;
        foreach (var l in _lights) l.gameObject.SetActive(activ);
    }

    public void SetEnableAudio(bool activ)
    {
        foreach (var item in _audioSources)
        {
            item.enabled = activ;
        }
    }
}
