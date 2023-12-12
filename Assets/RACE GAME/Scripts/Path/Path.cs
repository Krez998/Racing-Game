using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Path : MonoBehaviour
{
    public List<PathNode> Waypoints => _waypoints;

    [SerializeField] private List<PathNode> _waypoints;
    [SerializeField] private PathNode _pathNodePrefab;

    private Transform _newNodeTransform;

    public void AddNewWaypoint()
    {
        _newNodeTransform = transform;
        if (_waypoints.Count > 0)
            _newNodeTransform = _waypoints[_waypoints.Count - 1].transform;

        PathNode newTransform = Instantiate(_pathNodePrefab, _newNodeTransform.position, Quaternion.identity);
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
        if (_waypoints != null && _waypoints.Count > 1)
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < _waypoints.Count; i++)
            {
                Gizmos.DrawLine(_waypoints[i].transform.position, _waypoints[(i + 1) % _waypoints.Count].transform.position);
            }
        }
    }
}

[CustomEditor(typeof(Path))]
[CanEditMultipleObjects]
public class PathEditor : Editor
{   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Path path = (Path)target;
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
