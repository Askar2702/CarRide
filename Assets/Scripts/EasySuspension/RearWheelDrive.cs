using UnityEngine;
using System.Collections;

public class RearWheelDrive : MonoBehaviour
{


    [SerializeField] private bool fourWheelDrive;
    [SerializeField] private bool isMobileInput;
    public float maxAngle = 30;
    public float maxTorque = 300;
    public GameObject wheelShape;

    private WheelCollider[] wheels;
    private CarController _carController;

    float _angle = 0;
    float _torque = 0;

    // here we find all the WheelColliders down in the hierarchy
    public void Start()
    {
        wheels = GetComponentsInChildren<WheelCollider>();
        _carController = GetComponent<CarController>();

        for (int i = 0; i < wheels.Length; ++i)
        {
            var wheel = wheels[i];

            // create wheel shapes only when needed
            if (wheelShape != null)
            {
                var ws = GameObject.Instantiate(wheelShape);
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
        if (GameManager.instance.GameState == GameState.Play)
        {
            if (!isMobileInput)
            {
                _angle = maxAngle * Input.GetAxis("Horizontal");
                _torque = maxTorque * Input.GetAxis("Vertical");
            }
            else
            {
                _angle = maxAngle * _carController.Horizontal;
                _torque = maxTorque * _carController.Vertical;
            }
        }
        else if (GameManager.instance.GameState == GameState.Finish)
        {
            _angle = maxAngle * 1;
            _torque = maxTorque * 0;
        }

        foreach (WheelCollider wheel in wheels)
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
            if (wheelShape)
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

        foreach (WheelCollider wheel in wheels)
        {
            wheel.brakeTorque = _carController.CurrentBreakForse;
        }


    }
}
