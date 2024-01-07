using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceProgress : MonoBehaviour
{
    [SerializeField] private Car[] _cars;
    [SerializeField] private int _playerCarNumber;
    [SerializeField] private Dictionary<int, int> _carsScore = new Dictionary<int, int>();
    private KeyValuePair<int, int>[] _sortedDictionary;

    private void Awake()
    {
        _cars = FindObjectsOfType<Car>();

        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i].SetNumber(i);
            _carsScore.Add(_cars[i].Number, 0);

            if (_cars[i].IsPlayerCar)
                _playerCarNumber = _cars[i].Number;
        }
    }

    private void Start() => CheckPlayerPosition();

    public void AddPoint(int carNumber)
    {
        _carsScore[carNumber]++;
        CheckPlayerPosition();
    }

    private void CheckPlayerPosition()
    {
        _sortedDictionary = _carsScore.OrderByDescending(x => x.Value).ToArray();
        for (int i = 0; i < _sortedDictionary.Length; i++)
        {
            if (_sortedDictionary[i].Key == _playerCarNumber)
            {
                ProgresEvents.OnCheckpointReached?.Invoke(i + 1, _cars.Length);
                //foreach (var car in _carsScore)
                //{
                //    Debug.Log($"номер машины: {car.Key} очки: {car.Value} || твое место: {i + 1} твои очки: {_sortedDictionary[i].Value}");
                //}
            }
        }
    }
}
