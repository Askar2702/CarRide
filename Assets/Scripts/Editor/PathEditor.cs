using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    private PathCreator _creator;
    private Path Path
    {
        get
        {
            return _creator.path;
        }
    }
    private const float _segmentSelectDistanceThreshold = 0.1f;
    private int _selectedSegmentIndex = -1;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(_creator, "Create new");
            _creator.CreatePath();
        }
        bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");

        if (isClosed != Path.IsClosed)
        {
            Undo.RecordObject(_creator, "Toggle closed");
            Path.IsClosed = isClosed;
        }
        bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");

        if (autoSetControlPoints != Path.AutoSetControlPoints)
        {
            Undo.RecordObject(_creator, "Toggle Auto Set Control");
            Path.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
            SceneView.RepaintAll();
    }
    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    private void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (_selectedSegmentIndex != -1)
            {
                Undo.RecordObject(_creator, "Split Segment");
                Path.SplitSegment(mousePos, _selectedSegmentIndex);
            }
            else if (!Path.IsClosed)
            {
                Undo.RecordObject(_creator, "Add Segment");
                Path.AddSegment(mousePos);
            }
        }
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDistToAnchor = _creator.anchorDiameter * 0.5f;
            int closestAnchorIndex = -1;
            for (int i = 0; i < Path.NumPoints; i += 3)
            {
                float dist = Vector2.Distance(mousePos, Path[i]);
                if (dist < minDistToAnchor)
                {
                    minDistToAnchor = dist;
                    closestAnchorIndex = i;
                }
            }
            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(_creator, "Delete segment");
                Path.DeleteSegment(closestAnchorIndex);
            }
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDistToSegment = _segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;
            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                float dist = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dist < minDistToSegment)
                {
                    minDistToSegment = dist;
                    newSelectedSegmentIndex = i;
                }
            }
            if (newSelectedSegmentIndex != _selectedSegmentIndex)
            {
                _selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }
        HandleUtility.AddDefaultControl(0);
    }

    private void Draw()
    {
        for (int i = 0; i < Path.NumSegments; i++)
        {
            Vector2[] points = Path.GetPointsInSegment(i);
            if (_creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segmentColor = (i == _selectedSegmentIndex && Event.current.shift) ? _creator.selectedSegmentCol : _creator.segmentCol;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2);
        }

        for (int i = 0; i < Path.NumPoints; i++)
        {
            if (i % 3 == 0 || _creator.displayControlPoints)
            {
                Handles.color = (i % 3 == 0) ? _creator.anchorCol : _creator.controlCol;
                float handleSize = (i % 3 == 0) ? _creator.anchorDiameter : _creator.controlDiameter;
                Vector2 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (Path[i] != newPos)
                {
                    Undo.RecordObject(_creator, "MovePoint");
                    Path.MovePoint(i, newPos);
                }
            }
        }
    }
    private void OnEnable()
    {
        _creator = (PathCreator)target;
        if (_creator.path == null)
        {
            _creator.CreatePath();
        }
    }
}
