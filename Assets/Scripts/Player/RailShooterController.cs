using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class RailShooterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Transform _playerPos;
    [SerializeField] public CameraController _cameraController;
    [SerializeField] private List<Transform> _nodeList;
    [SerializeField] private int _index = 0;
    [SerializeField] private float _nextNodeDistance;



    [Header("Event")]
    [Tooltip("Si el jugador esta en un evento, no se puede mover, hasta que termine")]
    [SerializeField] public bool _duringEvent;
    [Tooltip("Lista de enemigos en espera para atacarme")]
    [SerializeField] public List<Enemy> _enemyList;
    [SerializeField] private float _cd;
    [SerializeField] private float _initialCd;

    void Start()
    {
        _duringEvent = false;
        _cameraController = player._cameraController;

        _enemyList = new List<Enemy>();

        foreach (Transform node in _nodeList) //Recorro la lista de nodos y asigno el pj como variable
        {
            NodeController nodeCont = node.gameObject.GetComponent<NodeController>();

            nodeCont._player = _playerPos.gameObject;
            nodeCont._railController = this;
        }

        SetNextNode();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cd < _initialCd) _cd += Time.deltaTime;

        CalculateNodeDistance();

        if(_cd >= _initialCd && _enemyList.Count > 0) //Agarro al primer enemigo de la lista, me ataca, reinicio el cd, y lo mando al final de la lista
        {
            EnemyAttack();
        }
        if(_enemyList.Count > 0 && !_cameraController._focus) //Tiene enemigos en espera
        {
            _cameraController._focus = true;
            //_cameraController.Focus();
        }
        else if (_enemyList.Count == 0 && _cameraController._focus) //No tiene enemigos en espera
        {
            _cameraController._focus = false;
            _cameraController.RemoveTarget();
            //_cameraController.Focus();
        }

        //_cameraController.Focus();

    }

    public void SetNextNode()
    {
        player._movement.SetTarget(_nodeList[_index].position);
    }

    public void CalculateNodeDistance() //Chequeo la distancia hacia el siguiente nodo
    {
        float nodeDistance = Vector3.Distance(_playerPos.position, _nodeList[_index].position);

        if (nodeDistance < _nextNodeDistance) //Si choco con el nodo al que estoy yendo
        {
            //Debug.Log("Llego al nodo " + _index);
            NodeController node = _nodeList[_index].GetComponent<NodeController>();

            if (node.IsEnemyTrigger() && !_duringEvent)
            {
                node.ActivateEvent();
                _duringEvent = true;
            }

            if (_index < _nodeList.Count - 1 && !_duringEvent)
            {
                _index++;
                SetNextNode();
            }
        }
    }

    public void EnemyAttack()
    {
        _cd = 0;
        Enemy firstEnemy = _enemyList[0];
        _cameraController.SetTarget(firstEnemy._focusTarget);
        _enemyList[0].Attack();
        _enemyList.RemoveAt(0);
        _enemyList.Insert(_enemyList.Count, firstEnemy);
    }

    public void SetDefaultCamera()
    {
        _cameraController.SetTarget(_cameraController.gameObject.transform);
    }


}
