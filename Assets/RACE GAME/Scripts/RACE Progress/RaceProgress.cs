using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class RaceProgress : MonoBehaviour
{
    [SerializeField] private CarProgress[] _cars;
    [SerializeField] private int _playerCarNumber;
    private WaitForSeconds _checkDelay;
    private CarProgress[] _sortedProgress;

    private void Awake()
    {
        _checkDelay = new WaitForSeconds(0.2f);
        _cars = FindObjectsOfType<CarProgress>();

        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i].SetNumber(i);

            if (_cars[i].IsPlayerCar)
                _playerCarNumber = _cars[i].Number;
        }
    }

    private void Start()
    {
        StartCoroutine(CheckPlayerPosition());
    }

    private IEnumerator CheckPlayerPosition()
    {
        while (true)
        {
            _sortedProgress = _cars
                .OrderByDescending(x => x.Points)
                .ThenBy(d => d.DistanceToCheckpoint)
                .ToArray();

            for (int i = 0; i < _sortedProgress.Length; i++)
            {
                if (_sortedProgress[i].Number == _playerCarNumber)
                {
                    GameEvents.OnCheckpointReached?.Invoke(i + 1, _cars.Length);
                }
            }

            yield return _checkDelay;
        }
    }
}