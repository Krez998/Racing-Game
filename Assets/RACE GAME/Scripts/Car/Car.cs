using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform CameraTarget => _cameraTarget;
    public float TiltX => _tiltX;

    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _tiltX;

    private void FixedUpdate()
    {
        if (_cameraTarget != null)
            _cameraTarget.position = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z); // смещение цели дл€ камеры
        CalculateCarTiltX();
    }

    public void SetCameraTarget()
    {
        var _cameraTarget = FindObjectOfType<ThirdPersonCamera>();
        if (_cameraTarget)
            _cameraTarget.SetTarget(this);
    }

    /// <summary>
    /// Ќаклон автомобил€ по оси X
    /// </summary>
    private void CalculateCarTiltX()
    {
        Vector3 verticVector = Vector3.up;
        _tiltX = Vector3.Angle(verticVector, transform.forward);
    }
}
