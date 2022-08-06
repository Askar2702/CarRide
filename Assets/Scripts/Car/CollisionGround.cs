using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGround : MonoBehaviour
{
    private CarSound _carSound;
    private void Start()
    {
        _carSound = GetComponent<CarSound>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GenerationMap>() || other.CompareTag("Finish")) _carSound.PlayCollisionClip();
    }
}
