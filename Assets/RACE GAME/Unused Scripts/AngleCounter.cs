using UnityEngine;

public class AngleCounter : MonoBehaviour
{   
    [SerializeField] private Transform object1;
    [SerializeField] private float _opponentAngle;

    private void FixedUpdate()
    {
        Vector3 targetPos = object1.position;
        targetPos.y = transform.position.y;
        _opponentAngle = Vector3.SignedAngle(targetPos - transform.position, transform.forward, Vector3.up);

        //DrawLines();
    }

    //private void DrawLines()
    //{
    //    Debug.DrawLine(transform.position, object1.position, Color.blue);
    //    Debug.DrawLine(transform.position, object2.position, Color.green);
    //}
}
