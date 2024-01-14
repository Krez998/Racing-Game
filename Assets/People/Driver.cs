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
            _animator.SetBool("LEFT", true);
            _animator.SetBool("RIGHT", false);
            _animator.SetBool("ALIGN", false);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _animator.SetBool("LEFT", false);
            _animator.SetBool("RIGHT", true);
            _animator.SetBool("ALIGN", false);
        }
        else
        {
            _animator.SetBool("LEFT", false);
            _animator.SetBool("RIGHT", false);
            _animator.SetBool("ALIGN", true);
        }
    }
}
