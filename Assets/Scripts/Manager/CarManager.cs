using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarManager : MonoBehaviour
{
    public static CarManager instance;
    [SerializeField] private Slider _sliderBarDistance;
    [SerializeField] private Finish _finish;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private CarController[] _cars;
    private float _distance;
    public CarController CurrentCar { get; private set; }
    private void Awake()
    {
        if (!instance) instance = this;
        CurrentCar = Instantiate(_cars[Game.instance.CurrentCar], _startPos, Quaternion.identity);
    }
    private void Start()
    {
        _sliderBarDistance.maxValue = _finish.transform.position.z - 50f;
        _sliderBarDistance.minValue = CurrentCar.transform.position.z;
    }
    private void Update()
    {
        _sliderBarDistance.value = CurrentCar.transform.position.z;
    }
    public void MoveForward(int i)
    {
        CurrentCar.MoveForward(i);
    }
    public void ApplyBreaking(bool activ)
    {
        CurrentCar.ApplyBreaking(activ);
    }

    public void SpawnShopCar(int id)
    {
        if (CurrentCar.Id == _cars[id].Id) return;
        var pos = CurrentCar.transform.position;
        pos = new Vector3(pos.x, pos.y + 2.5f, pos.z);
        Destroy(CurrentCar.gameObject);
        CurrentCar = Instantiate(_cars[Game.instance.CurrentCar], pos, Quaternion.identity);
    }
}
