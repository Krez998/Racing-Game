using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIRivalsDetector : MonoBehaviour
{
    //[SerializeField] private bool _detectorIsOn;
    //[SerializeField] private bool _showGizmos;
    //[SerializeField] private LayerMask _detectorLayerMask;
    //[SerializeField] private float _delectionDelay;
    //[SerializeField] private float _delectionRadius;
    //private WaitForSeconds _detectionSeconds;
    //[SerializeField] private int _numColliders;
    //[SerializeField] private Collider[] _colliders;


    //private IEnumerator DetectionCoroutine()
    //{
    //    while (true)
    //    {
    //        if (_detectorIsOn)
    //            PerformDetection();
    //        yield return _detectionSeconds;
    //    }
    //}


    //private void PerformDetection()
    //{
    //    _collidersLength = Physics.OverlapSphereNonAlloc(transform.position, _delectionRadius, _colliders, _detectorLayerMask);

    //    for (int i = 0; i < _collidersLength; i++)
    //    {
    //        if (_colliders[i].transform.position != transform.position)
    //        {
    //            _opponentAngle = Vector3.Angle(_colliders[i].transform.position - transform.position, transform.forward);
    //            CheckFrontAREA(_colliders[i]);

    //            float angle = Vector3.Angle(_colliders[i].transform.position - transform.position, transform.forward);
    //            float distance = Vector3.Distance(_colliders[i].transform.position - transform.position, transform.forward);
    //            //KATET = Mathf.Sign(angle) * distance;

    //            Vector3 objectPosition = _colliders[i].transform.position; // глобальные координаты объекта
    //            Vector3 localPosition = transform.InverseTransformPoint(objectPosition); // локальные координаты объекта относительно текущего объекта

    //        }
    //        else
    //            _selfCollider = _colliders[i];
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Handles.color = Color.yellow;
    //    if (_showGizmos)
    //        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _delectionRadius);
    //}
}
