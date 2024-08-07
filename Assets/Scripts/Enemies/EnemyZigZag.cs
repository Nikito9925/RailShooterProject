using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZigZag : Enemy
{
    [SerializeField] private bool _onZigZag;
    [SerializeField] private int _zigZagSegments;
    [SerializeField] private float _zigZagOffset;
    [SerializeField] private List<Vector3> _zigZagPoints;
    [SerializeField] private int _zigZagIndex;
    [SerializeField] private AttackMode _attackMode;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private Vector3 _originPos;
    [SerializeField] private bool _goingBack;

    [SerializeField] private bool _hasKnives;
    [SerializeField] private GameObject _proyectile;
    [SerializeField] private EnemyAudioController _audioController;

    public enum AttackMode
    {
        ZigZag,
        Knive
    }


    private void Start()
    {
        _isDeath = false;
        _hasKnives = true;
        _onZigZag = false;
        _goingBack = false;
        _zigZagPoints = new List<Vector3>();
        _zigZagIndex = 0;
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
                if(!_onZigZag && _goingBack && _agent.remainingDistance > .1f)
                {
                    transform.forward = Vector3.Lerp(transform.forward, (_player.position - transform.position).normalized, .95f * Time.deltaTime * 15f);
                }
                if(!_onZigZag && _goingBack && _agent.remainingDistance < .1f)                   //Si estoy volviendo para atras, y llego al punto. Cambio a modo Knive
                {
                    _goingBack = false;
                    _agent.SetDestination(transform.position);
                    _agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                    SwitchAttackMode();
                    _railController._cameraController.RemoveTarget();
                }

                if(_attackMode == AttackMode.ZigZag)                            //Si el tipo de ataque es ZigZag
                {
                    _agent.speed = 6f;
                    _agent.acceleration = 15f;
                    _agent.updateRotation = false;
                    _isWalking = false;

                    if (!_onZigZag && !_goingBack)                                             //Paso a estado de ZigZag, y calculo el camino
                    {
                        _onZigZag = true;
                        CalculateZigZagPath(transform.position, _player.position);
                        if(_railController._enemyList.Contains(this))
                        {
                            _railController._enemyList.Remove(this);
                        }
                    }
                    if (_onZigZag)                                              //Hago que mire al pj, y desactive las colisiones, para que no se tosquee
                    {
                        transform.forward = Vector3.Lerp(transform.forward, (_player.position - transform.position).normalized, .95f * Time.deltaTime * 15f);
                        _agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

                        if (_zigZagIndex < _zigZagPoints.Count) _agent.SetDestination(_zigZagPoints[_zigZagIndex]);     //Seteo de destino al nodo del patron

                        if (_agent.remainingDistance < .1f && _zigZagIndex < _zigZagPoints.Count - 1)                   //Si llego al nodo, paso al sig
                        {
                            Debug.Log("NEXT ZIG ZAG NODE");
                            _zigZagIndex++;
                            _agent.SetDestination(_zigZagPoints[_zigZagIndex]);
                        }

                        if(_agent.remainingDistance < _minDistance && _zigZagIndex == _zigZagPoints.Count - 1)          //Si llego al nodo final
                        {
                            if (!_railController._enemyList.Contains(this))     //Lo agrego primero en la lista
                            {
                                //_railController._enemyList.Add(this);
                                _railController._enemyList.Insert(0, this);
                                _railController.EnemyAttack();                  //Fuerzo el ataque
                            }
                        }
                    }
                }

                else if (_attackMode == AttackMode.Knive)                       //Si es tirar un cuchillo
                {
                    _agent.speed = 0;
                    _agent.SetDestination(_player.position);
                    _isWalking = false;
                    if (!_railController._enemyList.Contains(this))
                    {
                        _railController._enemyList.Add(this);
                    }
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
        switch(_attackMode)
        {
            case AttackMode.ZigZag:
                Debug.Log("ATTACK ZIGZAG");
                _audioController.PlayEnemyAttackSound();
                _animator.SetTrigger("AttackPunch");

                Invoke("GoBack", 1.5f);
                break;
            case AttackMode.Knive:
                Debug.Log("ATTACK KNIVE");
                _audioController.PlayEnemyThrowSound();
                _animator.SetTrigger("AttackKnive");

                Invoke("SwitchAttackMode", 1f);
                _railController._cameraController.RemoveTarget();
                break;
        }
        //_animator.SetTrigger("Attack");
        Debug.Log("Enemy Attack");
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

        _agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        _standBy = true;
        _animator.SetTrigger("Death");

        //Invoke("DestroyEnemy", 2f);
    }

    public override void UpdateState()
    {
        if (_standBy) _enemyState = EnemyState.Inactive;
        else if (!_standBy && _agent.remainingDistance >= _detectionDistance) _enemyState = EnemyState.Walking;
        else if (!_standBy && _agent.remainingDistance < _detectionDistance) _enemyState = EnemyState.WaitForAttack;
    }

    public void CalculateZigZagPath(Vector3 origin, Vector3 destiny)
    {
        _zigZagPoints.Clear();
        _originPos = transform.position;                                //Guardo la posicion original;
        Vector3 dir = (destiny - origin).normalized;                    //Calculo la direccion
        for(int i = 1; i < _zigZagSegments; i++)
        {
            float manteca = (float)i / _zigZagSegments;
            Vector3 point = Vector3.Lerp(origin, destiny, manteca);
            if(i % 2 == 0)
            {
                Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
                Vector3 newDir = rotation * dir;
                point += newDir * _zigZagOffset;
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);
                Vector3 newDir = rotation * dir;
                point += newDir * _zigZagOffset;
            }
            _zigZagPoints.Add(point);
        }
    }

    public void DeleteZigZagPath()
    {
        _zigZagPoints = new List<Vector3>();
        _zigZagIndex = 0;
    }

    private void OnDrawGizmos()
    {

        if(_zigZagPoints.Count > 0)
        {
            foreach(Vector3 point in _zigZagPoints)
            {
                Gizmos.DrawSphere(point, .2f);
            }
        }
    }

    public void SwitchAttackMode()
    {
        switch(_attackMode)
        {
            case AttackMode.ZigZag:
                _attackMode = AttackMode.Knive;
                break;
            case AttackMode.Knive:
                _attackMode= AttackMode.ZigZag;
                break;
        }
    }

    public void GoBack()
    {
        _onZigZag = false;
        _goingBack = true;
        _agent.speed = 5f;
        _agent.acceleration = 8f;
        _agent.updateRotation = false;

        DeleteZigZagPath();

        _agent.SetDestination(_originPos);

        if (_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }
    }

    public override void Shoot()
    {
        GameObject knive = Instantiate(_proyectile, _focusTarget.position, Quaternion.identity);
        Proyectile kniveController = knive.GetComponent<Proyectile>();

        kniveController.SetDir((Camera.main.transform.position - _focusTarget.position), 2f);
    }
}
