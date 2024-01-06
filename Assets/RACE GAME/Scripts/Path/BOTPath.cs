using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BOTPath : MonoBehaviour
{
    public List<Waypoint> Waypoints => _waypoints;
    [SerializeField] private bool _showLines;
    [SerializeField] private List<Waypoint> _waypoints;
    [SerializeField] private Waypoint _pathNodePrefab;

    private Transform _newNodeTransform;

    public void AddNewWaypoint()
    {
        _newNodeTransform = transform;
        if (_waypoints.Count > 0)
            _newNodeTransform = _waypoints[_waypoints.Count - 1].transform;

        Waypoint newTransform = Instantiate(_pathNodePrefab, _newNodeTransform.position, Quaternion.identity);
        newTransform.name = $"Waypoint {_waypoints.Count}";
        newTransform.transform.SetParent(transform);
        _waypoints.Add(newTransform);
    }

    public void RemoveLastWaypoint()
    {
        if (_waypoints.Count > 0)
        {
            DestroyImmediate(_waypoints[_waypoints.Count - 1].gameObject);
            _waypoints.Remove(_waypoints[_waypoints.Count - 1]);
        }
    }

    private void OnDrawGizmos()
    {
        if (_waypoints != null && _waypoints.Count > 1 && _showLines)
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < _waypoints.Count; i++)
            {
                Gizmos.DrawLine(_waypoints[i].transform.position, _waypoints[(i + 1) % _waypoints.Count].transform.position);
            }
        }
    }
}

[CustomEditor(typeof(BOTPath))]
[CanEditMultipleObjects]
public class PathEditor : Editor
{   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BOTPath path = (BOTPath)target;
        if (GUILayout.Button("Add new waypoint"))
        {
            path.AddNewWaypoint();
        }

        if (GUILayout.Button("Remove last waypoint"))
        {
            path.RemoveLastWaypoint();
        }
    }
}
