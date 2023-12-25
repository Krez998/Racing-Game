using System;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
    public event Action<Collider> EnteredTrigger;
    public event Action<Collider> ExitedTrigger;

    private void OnTriggerEnter(Collider other)
    {
        EnteredTrigger?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitedTrigger?.Invoke(other);
    }
}
