using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Car _playerCar;
    private void Awake()
    {
        Numbers.GenerateNums();
        //SpawnPlayerCar();
    }

    private void SpawnPlayerCar()
    {
        var car = Instantiate(_playerCar, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        car.SetCameraTarget();
    }

}
