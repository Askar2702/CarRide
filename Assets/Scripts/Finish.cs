using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out CarController car))
        {
            GameManager.instance.Finish();
        }
    }
}
