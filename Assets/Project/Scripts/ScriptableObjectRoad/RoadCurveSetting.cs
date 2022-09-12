using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataCurveRoad", menuName = "ScriptableObjectsRoadCurve", order = 1)]
public class RoadCurveSetting : ScriptableObject
{
    public RoadSetting[] RoadSettings;
}
