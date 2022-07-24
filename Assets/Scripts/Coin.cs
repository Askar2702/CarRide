using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartRotate());
    }
    private IEnumerator StartRotate()
    {
        while (true)
        {
            yield return null;
            transform.Rotate(Vector3.forward, 2f);
        }
    }


}
