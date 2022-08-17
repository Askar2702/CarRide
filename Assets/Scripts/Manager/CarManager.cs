using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarManager : MonoBehaviour
{
    [SerializeField] private Slider _sliderBarDistance;
    [SerializeField] private Finish _finish;
    private float _distance;
    private CarController _car;

    private void Start()
    {
        _car = FindObjectOfType<CarController>();
        _sliderBarDistance.maxValue = _finish.transform.position.z - 50f;
        _sliderBarDistance.minValue = _car.transform.position.z;
    }
    private void Update()
    {
        _sliderBarDistance.value = _car.transform.position.z;
    }
    public void MoveForward(int i)
    {
        _car.MoveForward(i);
    }
    public void ApplyBreaking(bool activ)
    {
        _car.ApplyBreaking(activ);
    }
}
