using UnityEngine;

[CreateAssetMenu(fileName ="Car", menuName = "Resouces/New Car")]
public class CarCharacteristics : ScriptableObject
{
    public string Name => _name;
    public int NumberOfGears => _numberOfGears;
    public int Horsepower => _horsepower;

    [SerializeField] private string _name;
    [SerializeField] private int _numberOfGears;
    [SerializeField] private int _horsepower;
    [SerializeField] private string _registrationPlate;
    [SerializeField] private float[] _torqueValues;
}
