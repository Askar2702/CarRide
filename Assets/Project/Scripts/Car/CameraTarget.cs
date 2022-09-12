using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTarget : MonoBehaviour
{
    void Start()
    {
        var cinema = FindObjectOfType<CinemachineVirtualCamera>();
        cinema.Follow = transform;
        cinema.LookAt = transform;
    }


}
