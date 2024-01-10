using System;

public class GameEvents
{
    public static Action<int> OnLapCompleted;
    public static Action<int, int> OnUpdatingPosition;
    public static Action<int> OnGearShifted;
    public static Action<float> OnSpeedometerUpdating;
    public static Action<float, GearBoxMode, float, float> OnSpeedIndicatorUpdating;
    public static Action<int, int> OnGameFinished;
}
