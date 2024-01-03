using UnityEngine;

[CreateAssetMenu(fileName ="Car", menuName = "Resouces/New Car")]
public class CarData : ScriptableObject
{
    public int VehicleId => _vehicleId;
    public string Name => _name;
    public float Mass => _mass;
    public WheelDriveMode WheelDriveMode => _wheelDriveMode;
    public float Speed => _speed;
    public int NumberOfGears => _numberOfGears;
    public AudioClip Acceleration=> _acceleration;
    public AudioClip Deceleration => _deceleration;
    public AudioClip Idle => _idle;

    [SerializeField] private int _vehicleId;
    [SerializeField] private string _name;
    [SerializeField] private float _mass;
    [SerializeField] private WheelDriveMode _wheelDriveMode;
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfGears;
    [SerializeField] private AudioClip _acceleration;
    [SerializeField] private AudioClip _deceleration;
    [SerializeField] private AudioClip _idle;

}
