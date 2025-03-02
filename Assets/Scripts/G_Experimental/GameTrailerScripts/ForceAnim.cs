using UnityEngine;

public class ForceAnim : MonoBehaviour
{
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("Start");
    }
}
