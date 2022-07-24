using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{
    public enum StateMove { Acceleration = 0, Zero = 1, Back = 2 }
    private enum StateSteeringWheel { Right = 0, Middle = 1, Left = 2 }


    private StateMove _stateMove;
    private StateSteeringWheel _stateSteeringWheel;
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    [SerializeField] private float _breakForse;
    public float CurrentBreakForse { get; private set; }
    [SerializeField] private Light[] _lights;

    public void SetSteer(float s)
    {
        Horizontal = s;
    }
    private void Start()
    {
        _stateMove = StateMove.Zero;
        _stateSteeringWheel = StateSteeringWheel.Middle;
    }

    private void Update()
    {
        if (GameManager.instance.GameState == GameState.Play)
        {
            Move();
            TurnSide();
            if (transform.position.y < -100f) GameManager.instance.Lose();
        }
    }



    private void TurnSide()
    {

        if (_stateSteeringWheel == StateSteeringWheel.Right)
        {
            Horizontal = Mathf.Clamp(Horizontal + Time.fixedDeltaTime, -1, 1);
        }
        else if (_stateSteeringWheel == StateSteeringWheel.Left)
        {
            Horizontal = Mathf.Clamp(Horizontal - Time.fixedDeltaTime, -1, 1);
        }
        else
        {
            if (Horizontal == 0) return;
            if (Horizontal < 0) Horizontal = Mathf.Clamp(Horizontal + Time.fixedDeltaTime, -1, 1);
            else Horizontal = Mathf.Clamp(Horizontal - Time.fixedDeltaTime, -1, 1);
        }
    }

    private void Move()
    {
        if (_stateMove == StateMove.Acceleration)
        {
            Vertical = Mathf.Clamp(Vertical + Time.fixedDeltaTime, -1, 1);
        }
        else if (_stateMove == StateMove.Back)
        {
            Vertical = Mathf.Clamp(Vertical - Time.fixedDeltaTime, -1, 1);
        }
        else
        {
            if (Vertical == 0) return;
            if (Vertical < 0) Vertical = Mathf.Clamp(Vertical + Time.fixedDeltaTime, -1, 1);
            else Vertical = Mathf.Clamp(Vertical - Time.fixedDeltaTime, -1, 1);
        }
    }
    public void MoveForward(int i)
    {
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
}
