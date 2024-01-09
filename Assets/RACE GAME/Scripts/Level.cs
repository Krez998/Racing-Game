using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Header("Init Settings")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerPosition _playerPosition;
    [SerializeField] private Car _playerCar;

    [Header("Game Rules")]
    [SerializeField] private int _laps;
    [SerializeField] private int _reward;

    [Header("Current PLayer Data")]
    [SerializeField] private int _currentPlayerRating;

    private void Awake()
    {
        Numbers.GenerateNums();
        //SpawnPlayerCar();

        if (_gameData != null)
        {
            _gameData.Load();
            _currentPlayerRating = _gameData.Data.Rating;
        }
    }


    private void OnEnable()
    {
        GameEvents.OnLapCompleted += CheckLapsCounter;
    }

    private void OnDisable()
    {
        GameEvents.OnLapCompleted -= CheckLapsCounter;
    }

    private void CheckLapsCounter(int laps)
    {
        if(_laps == laps)
            FinishGame();
    }

    private void FinishGame()
    {
        switch (_playerPosition.Position)
        {
            case 1:
                _reward = 10000;
                break;
            case 2:
                _reward = 5000;
                break;
            case 3:
                _reward = 2500;
                break;
            default:
                _reward = 1000;
                break;
        }

        Data data = new Data()
        {
            Rating = _currentPlayerRating + _reward
        };

        _gameData.Save(data);
    }

    //private void SpawnPlayerCar()
    //{
    //    var car = Instantiate(_playerCar, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    //    //car.SetCameraTarget();
    //}
}
