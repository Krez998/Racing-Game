using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerCars;

    public int chosen;

    private void Awake()
    {
        chosen = ChosenCar.CarIndex;
    }

    public void SpawnPlayer()
    {
        GameObject playerCar = Instantiate(_playerCars[ChosenCar.CarIndex]);
        playerCar.transform.position = new Vector3(0f, 0f, 20f);
    }
}
