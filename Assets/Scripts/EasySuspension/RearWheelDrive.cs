using UnityEngine;
using System.Collections;

public class RearWheelDrive : MonoBehaviour
{


    [SerializeField] private bool fourWheelDrive;
    [SerializeField] private bool isMobileInput;
    [SerializeField] private float _maxAngle = 30;
    [SerializeField] private float _maxTorque = 1500;

    [SerializeField] private float _maxSpeed;
    [SerializeField] private GameObject _wheelShape;

    private WheelCollider[] _wheels;
    private CarController _carController;

    float _angle = 0;
    float _torque = 0;

    // here we find all the WheelColliders down in the hierarchy
    public void Start()
    {
        _wheels = GetComponentsInChildren<WheelCollider>();
        _carController = GetComponent<CarController>();

        for (int i = 0; i < _wheels.Length; ++i)
        {
            var wheel = _wheels[i];

            // create wheel shapes only when needed
            if (_wheelShape != null)
            {
                var ws = GameObject.Instantiate(_wheelShape);
                ws.transform.parent = wheel.transform;
                if (wheel.transform.localPosition.x < 0f)
                {
                    ws.transform.localScale = new Vector3(ws.transform.localScale.x * -1f, ws.transform.localScale.y, ws.transform.localScale.z);
                }
            }
        }
    }

    // this is a really simple approach to updating wheels
    // here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
    // this helps us to figure our which wheels are front ones and which are rear
    public void Update()
    {
        // if (GameManager.instance.GameState == GameState.Play)
        {
            if (!isMobileInput)
            {
                _angle = _maxAngle * Input.GetAxis("Horizontal");
                _torque = Mathf.Clamp(_maxTorque * Input.GetAxis("Vertical"), -_maxSpeed, _maxSpeed);
            }
            else
            {
                _angle = _maxAngle * _carController.Horizontal;
                _torque = Mathf.Clamp(_maxTorque * _carController.Vertical, -_maxSpeed, _maxSpeed);
            }
        }
        if (GameManager.instance.GameState == GameState.Finish)
        {
            _angle = _maxAngle * 1;
        }

        foreach (WheelCollider wheel in _wheels)
        {
            // a simple car where front wheels steer while rear ones drive
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = _angle;

            if (fourWheelDrive) wheel.motorTorque = _torque;
            else
            {
                if (wheel.transform.localPosition.z < 0)
                    wheel.motorTorque = _torque;
            }

            // update visual wheels if any
            if (_wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // assume that the only child of the wheelcollider is the wheel shape
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = q;
            }

        }

        foreach (WheelCollider wheel in _wheels)
        {
            wheel.brakeTorque = _carController.CurrentBreakForse;
        }


    }
}
