using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private Vector3 _wishDir;
    [SerializeField] private float _speed;

    private void Start()
    {

    }
    void Update()
    {
        Move();
    }

    public void SetDir(Vector3 newDir, float speed)
    {
        _speed = speed;
        _wishDir = newDir.normalized;
    }

    public void Move()
    {
        transform.position += _wishDir * _speed * Time.deltaTime;
        transform.forward = _wishDir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("LOLOLOOLOLL");
            Destroy(gameObject);
            other.gameObject.GetComponent<Player>().DoDamage();
        }
    }
}
