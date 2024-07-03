using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnive : Enemy
{
    [Tooltip("Tiene cuchillos para tirar, en caso que no, golpea normal")]
    [SerializeField] private bool _hasKnives;
    [SerializeField] private float _throwDistance;
    [SerializeField] private GameObject _proyectile;
    [SerializeField] private EnemyAudioController _audioController;
    private void Start()
    {
        _isDeath = false;
        _life = 10;
        _hasKnives = true;
        _agent.SetDestination(_player.position);

        BoxCollider[] colliderList = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliderList)
        {
            string colliderTag = collider.tag;
            string manteca = colliderTag.Substring(8);

            EnemyPart enumPart = (EnemyPart)Enum.Parse(typeof(EnemyPart), manteca);

            _bodyParts.Add(enumPart, collider);
        }
    }

    void Update()
    {
        _animator.SetBool("Walking", _isWalking);
        UpdateState();

        switch(_enemyState)
        {
            case EnemyState.Inactive:

                _agent.speed = 0f;
                _isWalking = false;
                break;
            case EnemyState.Walking:
                _agent.speed = _speed;
                _agent.SetDestination(_player.position);
                _isWalking = true;
                if (_railController._enemyList.Contains(this))
                {
                    _railController._enemyList.Remove(this);
                }
                break;
            case EnemyState.WaitForAttack:
                _agent.speed = 0;
                _agent.SetDestination(_player.position);
                _isWalking = false;
                if(!_railController._enemyList.Contains(this))
                {
                    _railController._enemyList.Add(this);
                }
                break;
        }

        if(_life <= 0 && !_isDeath)
        {
            _isDeath = true;
            Death();
        }
    }

    public override void Attack()
    {
        _animator.SetTrigger("Attack");
        Debug.Log("Enemy Attack");
    }

    public override void Shoot()
    {
        GameObject knive = Instantiate(_proyectile, _focusTarget.position, Quaternion.identity);
        Proyectile kniveController = knive.GetComponent<Proyectile>();

        kniveController.SetDir((Camera.main.transform.position - _focusTarget.position), 2f);
    }

    public override void Death()
    {
        _audioController.PlayEnemyDeathSound();

        if (_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }
        if (_nodeController != null && _nodeController._enemyList.Contains(this.gameObject)) //Si pertenece a un nodo, y esta en la lista
        {
            _nodeController._enemyList.Remove(this.gameObject);
        }

        _standBy = true;
        _animator.SetTrigger("Death");

        Invoke("DestroyEnemy", 3f);
    }

    public override void UpdateState()
    {
        if (_standBy) _enemyState = EnemyState.Inactive;
        else if (!_standBy && _hasKnives)
        {
            if (_agent.remainingDistance >= _throwDistance) _enemyState = EnemyState.Walking;
            else if (_agent.remainingDistance < _throwDistance) _enemyState = EnemyState.WaitForAttack;
        }
        else if(!_standBy && !_hasKnives)
        {
            if (_agent.remainingDistance >= _minDistance) _enemyState = EnemyState.Walking;
            else if (_agent.remainingDistance < _minDistance) _enemyState = EnemyState.WaitForAttack;
        }
    }
}
