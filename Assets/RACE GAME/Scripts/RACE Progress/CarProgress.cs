using System.Linq;
using UnityEngine;

public class CarProgress : MonoBehaviour
{
    public bool IsPlayerCar => _isPlayerCar;
    public int RacingNumber => _racingNumber;
    public int CheckpointsCompleted => _checkpointsCompleted;
    public int Laps => _laps;
    public float DistanceToCheckpoint => _distanceToCheckpoint;

    [SerializeField] private LayerMask _checkpointLayer;
    [SerializeField] private int _racingNumber;
    [SerializeField] private Transform[] _checkpoints;
    [SerializeField] private Transform _targetCheckpoint;
    [SerializeField] private int _targetCheckpointIndex;
    [SerializeField] private float _distanceToCheckpoint;
    [SerializeField] private int _checkpointsCompleted;
    [SerializeField] private int _laps;
    private bool _isPlayerCar;

    public void GetData(bool isPlayerCar)
    {
        _isPlayerCar = isPlayerCar;
    }

    public void SetRacingNumber(int racingNumber) => _racingNumber = racingNumber;

    private void Awake()
    {
        GameObject container = GameObject.FindGameObjectWithTag("CheckpointContainer");
        _checkpoints = container.GetComponentsInChildren<Transform>()
            .Where(x => (1 << x.gameObject.layer & _checkpointLayer) != 0)
            .ToArray();
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
            _checkpointsCompleted++;
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