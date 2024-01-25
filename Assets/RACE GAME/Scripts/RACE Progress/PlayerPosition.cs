using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public int Position => _position;

    [SerializeField] private CarProgress[] _cars;
    [SerializeField] private int _playerRacingNumber;
    [SerializeField] private int _position;
    [SerializeField] private CarProgress[] _sortedProgress;
    private WaitForSeconds _checkDelay;

    private void Awake()
    {
        _checkDelay = new WaitForSeconds(0.2f);
    }

    public void FindAllCars()
    {
        _cars = FindObjectsOfType<CarProgress>();

        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i].SetRacingNumber(i);

            if (_cars[i].IsPlayerCar)
                _playerRacingNumber = _cars[i].RacingNumber;
        }

        StartCoroutine(CheckPlayerPosition());
    }

    private IEnumerator CheckPlayerPosition()
    {
        while (true)
        {
            _sortedProgress = _cars
                .OrderByDescending(x => x.CheckpointsCompleted)
                .ThenBy(d => d.DistanceToCheckpoint)
                .ToArray();

            for (int i = 0; i < _sortedProgress.Length; i++)
            {
                if (_sortedProgress[i].RacingNumber == _playerRacingNumber)
                {
                    _position = i + 1;
                    GameEvents.OnUpdatingPosition?.Invoke(_position, _cars.Length);
                }
            }

            yield return _checkDelay;
        }
    }
}