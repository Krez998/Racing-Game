using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float TimeRemainingToStartRace => _timeRemainingToStartRace;
    public int Laps => _laps;

    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private PlayerPosition _playerPosition;

    [Header("Правила игры")]
    [SerializeField] private float _timeRemainingToStartRace;
    [SerializeField] private int _laps;

    [Header("Текущие данные игрока")]
    [SerializeField] private int _currentPlayerRating;

    private void Awake()
    {
        Numbers.GenerateNums();
        _gameData.Load();
        _currentPlayerRating = _gameData.Data.Rating;
        _playerSpawner.SpawnPlayer();
        _playerPosition.FindAllCars();
        StartCoroutine(StartTimerToStartRace());
    }

    private void OnEnable()
    {
        GameEvents.OnLapCompleted += CheckLapsCounter;
    }

    private void OnDisable()
    {
        GameEvents.OnLapCompleted -= CheckLapsCounter;
    }

    private IEnumerator StartTimerToStartRace()
    {
        while (true)
        {
            _timeRemainingToStartRace -= 1;

            if (_timeRemainingToStartRace <= 0)
            {
                StartRace();
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void CheckLapsCounter(int laps)
    {
        if(_laps == laps)
            FinishRace();
    }

    private void StartRace()
    {
        GameEvents.OnRaceStarted?.Invoke();
    }

    private void FinishRace()
    {
        int reward;
        switch (_playerPosition.Position)
        {
            case 1:
                reward = 10000;
                break;
            case 2:
                reward = 5000;
                break;
            case 3:
                reward = 2500;
                break;
            default:
                reward = 1000;
                break;
        }

        GameEvents.OnRaceFinished?.Invoke(_playerPosition.Position, reward);

        Data data = new Data()
        {
            Rating = _currentPlayerRating + reward
        };

        _gameData.Save(data);        
    }
}
