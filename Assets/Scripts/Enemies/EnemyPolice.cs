using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPolice : Enemy
{
    [SerializeField] private EnemyAudioController _audioController;
    [SerializeField] private GameObject explosionPrefab;
    public Player player;
    private void Start()
    {
        _isDeath = false;
        _life = 10;

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
        _audioController.PlayEnemyAttackSound();
    }

    public override void Death()
    {
        _audioController.PlayEnemyDeathSound();

        if (_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }
        if (_nodeController != null && _nodeController._enemyList.Contains(this.gameObject))
        {
            _nodeController._enemyList.Remove(this.gameObject);
        }

        /*Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player != null)
        {
            player.DoDamage();
        }*/
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float maxDistance = 3f;

        if (distanceToPlayer <= maxDistance)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player != null)
            {
                player.DoDamage();
            }
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        _standBy = true;
        _animator.SetTrigger("Death");

        Destroy(explosionPrefab, 3f);
        Invoke("DestroyEnemy", 2f);

    }

    public override void UpdateState()
    {
        if (_standBy) _enemyState = EnemyState.Inactive;
        else if (!_standBy && _agent.remainingDistance >= _minDistance) _enemyState = EnemyState.Walking;
        else if (!_standBy && _agent.remainingDistance < _minDistance) _enemyState = EnemyState.WaitForAttack;
    }
}