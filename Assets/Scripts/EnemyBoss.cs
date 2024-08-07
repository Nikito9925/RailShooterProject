using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    [SerializeField] private EnemyAudioController _audioController;
    [SerializeField] private Vector3 _originalPos;
    [SerializeField] private bool _goBack;

    [SerializeField] CanvasController _canvas;
    private void Start()
    {
        _isDeath = false;
        _life = 200;

        _agent.SetDestination(_player.position);

        BoxCollider[] colliderList = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliderList)
        {
            string colliderTag = collider.tag;
            string manteca = colliderTag.Substring(8);

            EnemyPart enumPart = (EnemyPart)Enum.Parse(typeof(EnemyPart), manteca);

            _bodyParts.Add(enumPart, collider);
        }

        _canvas.EnableBoss(true);

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

                if(_goBack && _agent.remainingDistance > .1f)
                {
                    transform.forward = Vector3.Lerp(transform.forward, (_player.position - transform.position).normalized, .95f * Time.deltaTime * 15f);
                }

                if (_goBack && _agent.remainingDistance < .1f)                   //Si estoy volviendo para atras, y llego al punto.
                {
                    _goBack = false;
                    _agent.SetDestination(transform.position);
                    _agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                    _enemyState = EnemyState.Walking;
                    //_railController._cameraController.RemoveTarget();
                }

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
            //Debug.Log("LIFE == 0");
            _isDeath = true;
            Death();
        }


    }

    public override void Attack()
    {
        _animator.SetTrigger("Attack");
        Debug.Log("Enemy Attack");
        _audioController.PlayEnemyAttackSound();
        _goBack = !_goBack;
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

        //Invoke("DestroyEnemy", 3f);

    }

    public override void UpdateState()
    {
        if (_standBy) _enemyState = EnemyState.Inactive;
        else if (!_standBy && _agent.remainingDistance >= _minDistance) _enemyState = EnemyState.Walking;
        else if (!_standBy && _agent.remainingDistance < _minDistance) _enemyState = EnemyState.WaitForAttack;
    }

    public override void DoDamage(EnemyPart bodyPart, RaycastHit hit)
    {
        switch (bodyPart)
        {
            case EnemyPart.Head:
                _life -= 10;
                //DeleteBodyPart(bodyPart);

                break;
            case EnemyPart.Chest:
            case EnemyPart.Abs:
                _life -= 2;
                //if (_life < 0) DeleteBodyPart(bodyPart);

                break;
            case EnemyPart.Pelvis:
                _life -= 2;

                break;
            case EnemyPart.RThigh:
            case EnemyPart.RLeg:
            case EnemyPart.LThigh:
            case EnemyPart.LLeg:
                _life -= 2;

                break;
            case EnemyPart.RUpperArm:
                _life -= 2;
                //if (_life < 8 && _bodyParts.ContainsKey(EnemyPart.RForeArm)) DeleteBodyPart(EnemyPart.RForeArm);

                break;
            case EnemyPart.LUpperArm:
                _life -= 2;
                //if (_life < 8 && _bodyParts.ContainsKey(EnemyPart.LForeArm)) DeleteBodyPart(EnemyPart.LForeArm);

                break;
            case EnemyPart.RForeArm:
                _life -= 2;
                //DeleteBodyPart(bodyPart);

                break;
            case EnemyPart.LForeArm:
                _life -= 2;
                //DeleteBodyPart(bodyPart);

                break;
        }

        HitEffect(hit);

        if (_life > 0)
        {
            //_animator.SetFloat("HitMode", Random.Range(0f, 1f));                        //En caso de querer mezclar las animaciones para q no se tan repetitivo
            //_animator.SetFloat("HitMode", .5f);                                       //Usar el random range, sino setear .5 para la default
            //_animator.SetTrigger("GetHit");
        }


        _canvas.UpdateBossCanvas((int)_life, 200);
    }

    public void GoBack()
    {
        //_onZigZag = false;
        _goBack = true;
        _agent.speed = 5f;
        _agent.acceleration = 8f;
        _agent.updateRotation = false;

        //DeleteZigZagPath();

        _agent.SetDestination(_originalPos);

        if (_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }
    }

}
