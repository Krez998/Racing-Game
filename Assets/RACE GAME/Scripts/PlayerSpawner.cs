using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerCars;
    [SerializeField] private int _chosenCarIndex;

    private void Awake()
    {
        _chosenCarIndex = ChosenCar.CarIndex;
    }

    public void SpawnPlayerCar()
    {
        //GameObject playerCar = Instantiate(_playerCars.Select(car => car).Where(data => data.CarData.Id == _chosenCarIndex).First());
        GameObject playerCar = Instantiate(_playerCars[ChosenCar.CarIndex]);
        playerCar.transform.position = new Vector3(0f, 0f, 18f);
    }
}
