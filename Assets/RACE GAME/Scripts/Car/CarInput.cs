using UnityEngine;

[RequireComponent(typeof(CarEngine))]
public class CarInput : MonoBehaviour
{
    [SerializeField, Range(0, 40)] private float _rotationAngle;
    [SerializeField] private bool _allowMove = false;
    private IMovable _movable;
    private ISteerable _steerable;

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
        _steerable = GetComponent<ISteerable>();
    }

    private void OnEnable() => GameEvents.OnRaceStarted += AllowMove;
    private void OnDisable() => GameEvents.OnRaceStarted -= AllowMove;

    private void AllowMove() => _allowMove = true;

    private void Update()
    {
        if (_allowMove)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                _movable.Acceleration();
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                _movable.Reverse();
            else if (Input.GetKey(KeyCode.Space))
                _movable.Brake();
            else
                _movable.Deceleration();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            _steerable.TurnLeft(_rotationAngle);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _steerable.TurnRight(_rotationAngle);
        else
            _steerable.StraightenSteeringWheel();

    }
}

