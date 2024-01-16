using UnityEngine;

public class Driver : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _animator.SetBool("Left", true);
            _animator.SetBool("Right", false);
            _animator.SetBool("Align", false);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _animator.SetBool("Left", false);
            _animator.SetBool("Right", true);
            _animator.SetBool("Align", false);
        }
        else
        {
            _animator.SetBool("Left", false);
            _animator.SetBool("Right", false);
            _animator.SetBool("Align", true);
        }
    }
}
