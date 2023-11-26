using UnityEngine;

[RequireComponent(typeof(CarEngine))]
public class CarInput : MonoBehaviour
{
    [SerializeField, Range(0, 40)] private float _rotationAngle;
    IMovable _movable;
    ISteerable _steerable;

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
        _steerable = GetComponent<ISteerable>();
    }

    private void FixedUpdate()
    {
        // Позже переделать под New Input System
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            _movable.Acceleration();
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            _movable.Reverse();
        else
            _movable.Deceleration();

        if (Input.GetKey(KeyCode.Space))
            _movable.Brake();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            _steerable.TurnLeft(_rotationAngle);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _steerable.TurnRight(_rotationAngle);
        else
            _steerable.StraightenSteeringWheel();

    }
}

