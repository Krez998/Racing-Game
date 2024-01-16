using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    [SerializeField] private int CarIndex;

    [SerializeField] private GameData gameData;

    [SerializeField] private CarData _carData;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;


    private void Start()
    {
        _image.sprite = _carData.Image;

        if (gameData.Data.Rating < _carData.TargetRaiting)
        {
            _button.interactable = false;
            _buttonText.SetText("Недостаточно рейтинга");
        }
        else
        {
            _button.interactable = true;
            _buttonText.SetText("Выбрать");
        }


        Debug.Log(gameData.Data.Rating);
    }

    public void ChooseCar()
    {
        //ChosenCar.CarIndex = _carData.UID;
        ChosenCar.CarIndex = CarIndex;
    }

    //public void ButtonCar1()
    //{
    //    ChosenCar.CarIndex = 0;
    //}

    //public void ButtonCar2()
    //{
    //    ChosenCar.CarIndex = 1;
    //}

    //public void ButtonCar3()
    //{
    //    ChosenCar.CarIndex = 2;
    //}

    //public void ButtonCar4()
    //{
    //    ChosenCar.CarIndex = 3;
    //}
}
