using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class CheckpointDetector : MonoBehaviour
{
    [SerializeField] private Checkpoint[] _checkpoints;
    [SerializeField] private Vector3 _targetCheckpoint;
    [SerializeField] private List<Checkpoint> _completedCheckpoint;
    [SerializeField] private int _laps;
    private RaceProgress _raceProgress;
    private Car _car;

    private void Awake()
    {
        _car = GetComponent<Car>();
        _checkpoints = FindObjectsOfType<Checkpoint>();
        _raceProgress = FindObjectOfType<RaceProgress>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Checkpoint component) && !_completedCheckpoint.Contains(component))
        {
            _completedCheckpoint.Add(component);

            if (_completedCheckpoint.Count == _checkpoints.Length)
            {
                _completedCheckpoint.Clear();
                _laps++;

                if (_car.IsPlayerCar)
                    ProgresEvents.OnLapCompleted?.Invoke(_laps);
            }

            if (_raceProgress != null)
                _raceProgress.AddPoint(_car.Number);

        }
    }

    private void FindFirstCheckpoint()
    {
        _targetCheckpoint = _checkpoints[0].transform.position;
    }
}