using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerCars;
    [SerializeField] private int _chosenCarIndex;

    private void Start()
    {
        _chosenCarIndex = ChosenCar.CarIndex;
    }

    public void SpawnPlayerCar()
    {
        //GameObject playerCar = Instantiate(_playerCars.Select(car => car).Where(data => data.CarData.Id == _chosenCarIndex).First());
        GameObject playerCar = Instantiate(_playerCars[ChosenCar.CarIndex]);
        playerCar.transform.position = transform.position;
    }
}
