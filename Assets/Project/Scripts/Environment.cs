using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private Material _fogMat;
    [ColorUsage(true, true)]
    [SerializeField] private Color[] _fogColor;
    public int index;
    void Start()
    {
        _fogMat.SetColor("_FogColor", _fogColor[Random.Range(0, _fogColor.Length)]);
    }


}
