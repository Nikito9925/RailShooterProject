using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void DestroyBlood()
    {
        Destroy(gameObject);
    }
}
