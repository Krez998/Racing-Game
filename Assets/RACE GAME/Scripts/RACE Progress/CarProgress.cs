using System.Linq;
using UnityEngine;

public class CarProgress : MonoBehaviour
{
    public bool IsPlayerCar => _isPlayerCar;
    public int Number => _number;
    public int Points => _points;
    public float DistanceToCheckpoint => _distanceToCheckpoint;

    [SerializeField] private LayerMask _checkpointLayer;
    [SerializeField] private int _number;
    [SerializeField] private Transform[] _checkpoints;
    [SerializeField] private Transform _targetCheckpoint;
    [SerializeField] private int _targetCheckpointIndex;
    [SerializeField] private float _distanceToCheckpoint;
    [SerializeField] private int _points;
    [SerializeField] private int _laps;
    private bool _isPlayerCar;

    public void GetData(bool isPlayerCar)
    {
        _isPlayerCar = isPlayerCar;
    }

    public void SetNumber(int number) => _number = number;

    private void Awake()
    {
        _checkpoints = FindObjectsOfType<Transform>()
            .Where(x => (1 << x.gameObject.layer & _checkpointLayer) != 0)
            .OrderBy(x => x.name).ToArray();
    }

    private void Start() => AssignFirstCheckpoint();

    private void Update()
    {
        _distanceToCheckpoint = Vector3.Distance(transform.position, _targetCheckpoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Transform component)
            && (((1 << component.gameObject.layer) & _checkpointLayer) != 0)
            && _targetCheckpoint == component.transform)
        {
            _points++;
            _targetCheckpointIndex++;

            if (_targetCheckpointIndex == _checkpoints.Length)
            {
                _targetCheckpointIndex = 0;
                _laps++;

                if (IsPlayerCar)
                    GameEvents.OnLapCompleted?.Invoke(_laps);
            }

            _targetCheckpoint = _checkpoints[_targetCheckpointIndex].transform;
        }
    }

    private void AssignFirstCheckpoint()
    {
        _targetCheckpointIndex = 0;
        _targetCheckpoint = _checkpoints[_targetCheckpointIndex].transform;
    }
}