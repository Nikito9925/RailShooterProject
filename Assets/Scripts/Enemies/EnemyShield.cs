using System;
using UnityEngine;

public class EnemyShield : Enemy
{
    [SerializeField] private bool _hasShield;
    private void Start()
    {
        _hasShield = true;
        _life = 10;

        _agent.SetDestination(_player.position);

        BoxCollider[] colliderList = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliderList)
        {
            if(collider.TryGetComponent<ShieldController>(out ShieldController shield))
            {
                break;
            }
            else
            {
                string colliderTag = collider.tag;
                string manteca = colliderTag.Substring(8);

                EnemyPart enumPart = (EnemyPart)Enum.Parse(typeof(EnemyPart), manteca);

                _bodyParts.Add(enumPart, collider);
            }
        }
    }

    void Update()
    {
        _animator.SetBool("Walking", _isWalking);
        UpdateState();

        switch (_enemyState)
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
                if (!_railController._enemyList.Contains(this))
                {
                    _railController._enemyList.Add(this);
                }
                break;
        }

        if (_life <= 0 && !_isDeath)
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

    public override void Death()
    {
        if (_nodeController == null) //Si no pertenece a un nodo
        {
            Destroy(this.gameObject);
        }
        else if (!(_nodeController == null) && _nodeController._enemyList.Contains(this.gameObject)) //Si pertenece a un nodo, y esta en la lista
        {
            _nodeController._enemyList.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

        if (_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }

        if(_hasShield)
        {
            ShieldController shield = GetComponentInChildren<ShieldController>();
            shield.DestroyShield();
        }
    }

    public void BackToNormal()
    {
        _hasShield = false;

    }

    public override void UpdateState()
    {
        if (_standBy) _enemyState = EnemyState.Inactive;
        else if (!_standBy && _agent.remainingDistance >= _minDistance) _enemyState = EnemyState.Walking;
        else if (!_standBy && _agent.remainingDistance < _minDistance) _enemyState = EnemyState.WaitForAttack;
    }
}
