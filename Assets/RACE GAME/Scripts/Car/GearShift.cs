using UnityEngine;

[RequireComponent(typeof(CarEngine))]
public class GearShift : MonoBehaviour, IGearShift
{
    [SerializeField] private CarCharacteristics _carCharacteristics;
    [SerializeField] int _currentGear;

    public void GearShiftUp()
    {
        Debug.Log("GEAR UP");
    }

    public void GearShiftDown()
    {
        Debug.Log("GEAR DOWN");
    }

}
