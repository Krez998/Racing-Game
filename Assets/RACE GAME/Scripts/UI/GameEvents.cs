using System;

public class GameEvents
{
    public static Action<int> OnLapCompleted;
    public static Action<int, int> OnCheckpointReached;
    public static Action<int> OnGearShifted;
    public static Action<float> OnSpeedometerUpdating;
    public static Action<float, GearBoxMode, float, float> OnSpeedIndicatorUpdating;
}
