using System;

public class ProgresEvents
{
    public static Action<int> OnLapCompleted;
    public static Action<int, int> OnCheckpointReached;
}
