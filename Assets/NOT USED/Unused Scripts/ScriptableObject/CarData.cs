using UnityEngine;

[CreateAssetMenu(fileName ="Car", menuName = "Resouces/New Car")]
public class CarData : ScriptableObject
{
    public int TargetRaiting => _targetRaiting;
    public Sprite Image => _image; 
    public string Name => _name;
    public float Mass => _mass;
    public WheelDriveMode WheelDriveMode => _wheelDriveMode;
    public float Speed => _speed;
    public int NumberOfGears => _numberOfGears;
    public float MotorTorque => _motorTorque;
    public float BrakeTorque => _brakeTorque;
    public AudioClip Acceleration=> _acceleration;
    public AudioClip Deceleration => _deceleration;
    public AudioClip Idle => _idle;

    [SerializeField] private int _targetRaiting;
    [SerializeField] private Sprite _image;
    [SerializeField] private string _name;
    [SerializeField] private float _mass;
    [SerializeField] private WheelDriveMode _wheelDriveMode;
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfGears;
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _brakeTorque;
    [SerializeField] private AudioClip _acceleration;
    [SerializeField] private AudioClip _deceleration;
    [SerializeField] private AudioClip _idle;

//    public string UID;

//    private void OnValidate()
//    {
//        if (string.IsNullOrWhiteSpace(UID))
//        {
//            AssignNewUID();
//        }
//    }

//    private void Reset()
//    {
//        AssignNewUID();
//    }

//    public void AssignNewUID()
//    {
//        UID = System.Guid.NewGuid().ToString();
//#if UNITY_EDITOR
//        UnityEditor.EditorUtility.SetDirty(this);
//#endif
//    }
}
