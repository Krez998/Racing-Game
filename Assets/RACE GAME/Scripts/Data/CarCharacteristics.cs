using UnityEngine;

[CreateAssetMenu(fileName ="Car", menuName = "Resouces/New Car")]
public class CarCharacteristics : ScriptableObject
{
    public int VehicleId => _vehicleId;
    public string Name => _name;
    public float Speed => _speed;
    public int NumberOfGears => _numberOfGears;

 

    [SerializeField] private int _vehicleId;
    [SerializeField] private string _name;
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfGears;

    //[SerializeField] private string _registrationPlate;
    
}
