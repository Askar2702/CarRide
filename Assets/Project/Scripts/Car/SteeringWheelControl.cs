using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringWheelControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool _wheelbeingheld = false;
    [SerializeField] private RectTransform _wheel;
    private float _wheelAngle = 0f;
    private float _lastWheelAngle = 0f;
    private Vector2 _center;
    [SerializeField] private float _maxSteerAngle = 200f;
    [SerializeField] private float _releaseSpeed = 300f;
    public float OutPut;

    void Update()
    {
        if (!_wheelbeingheld && _wheelAngle != 0f)
        {
            float DeltaAngle = _releaseSpeed * Time.deltaTime;
            if (Mathf.Abs(DeltaAngle) > Mathf.Abs(_wheelAngle))
                _wheelAngle = 0f;
            else if (_wheelAngle > 0f)
                _wheelAngle -= DeltaAngle;
            else
                _wheelAngle += DeltaAngle;
        }
        _wheel.localEulerAngles = new Vector3(0, 0, -_maxSteerAngle * OutPut);
        OutPut = _wheelAngle / _maxSteerAngle;
        CarManager.instance.CurrentCar.SetSteer(OutPut);
    }
    public void OnPointerDown(PointerEventData data)
    {
        _wheelbeingheld = true;
        _center = RectTransformUtility.WorldToScreenPoint(data.pressEventCamera, _wheel.position);
        _lastWheelAngle = Vector2.Angle(Vector2.up, data.position - _center);
    }
    public void OnDrag(PointerEventData data)
    {
        float NewAngle = Vector2.Angle(Vector2.up, data.position - _center);
        if ((data.position - _center).sqrMagnitude >= 400)
        {
            if (data.position.x > _center.x)
                _wheelAngle += NewAngle - _lastWheelAngle;
            else
                _wheelAngle -= NewAngle - _lastWheelAngle;
        }
        _wheelAngle = Mathf.Clamp(_wheelAngle, -_maxSteerAngle, _maxSteerAngle);
        _lastWheelAngle = NewAngle;
    }
    public void OnPointerUp(PointerEventData data)
    {
        OnDrag(data);
        _wheelbeingheld = false;
    }
}
