using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Material _mat;
    [SerializeField] private Color[] _color;
    void Start()
    {
        _mat.color = _color[Random.Range(0, _color.Length)];
    }


}
