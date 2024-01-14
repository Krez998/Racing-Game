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
        GameObject playerCar = Instantiate(_playerCars[_chosenCarIndex]);
        playerCar.transform.position = new Vector3(0f, 0f, 18f);
    }
}
