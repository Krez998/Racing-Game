using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelection : MonoBehaviour
{
    public void ButtonCar1()
    {
        ChosenCar.CarIndex = 0;
    }

    public void ButtonCar2()
    {
        ChosenCar.CarIndex = 1;
    }

    public void ButtonCar3()
    {
        ChosenCar.CarIndex = 2;
    }

    public void ButtonCar4()
    {
        ChosenCar.CarIndex = 3;
    }
}
