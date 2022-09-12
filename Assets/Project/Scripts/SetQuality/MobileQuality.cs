using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MobileQuality : MonoBehaviour
{
    [SerializeField] private float _renderScale;
    void Start()
    {
        // var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        // urpAsset.renderScale = 0.1f;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }


}
