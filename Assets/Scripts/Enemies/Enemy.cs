using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] protected float _minDistance;        //Distancia minima hasta que me ataca
    [SerializeField] protected float _speed;
    [SerializeField] protected float _life;

    [Header("References")]
    [SerializeField] public Transform _focusTarget;     //A donde focusea la camara cuando el enemigo ataca
    [SerializeField] protected Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected RailShooterController _railController;
    [SerializeField] protected NodeController _nodeController;      //Referencia del nodo, en caso de que el enemigo spawne por evento
    [SerializeField] protected AnimEventController _animEvent;
    [SerializeField] protected ParticleSystem _bloodParticle;
    [SerializeField] protected ParticleSystem _bloodImpact;

    [Header("Player")]
    [SerializeField] protected Transform _player;

    [Header("Bools")]
    [SerializeField] protected bool _standBy;
    [SerializeField] protected bool _isWalking;
    [SerializeField] protected bool _isDeath;

    [Header("State Machine")]
    [SerializeField] protected EnemyState _enemyState;

    [Header("Body Parts")]
    [SerializeField] protected Dictionary<EnemyPart, Collider> _bodyParts; 

    public enum EnemyState                              //State Machine
    {
        Inactive,
        Walking,
        WaitForAttack,
        Attacking,
    }

    public enum EnemyPart
    {
        Head,               //Cabeza

        Chest,              //Pecho
        Abs,                //Abdominales
        Pelvis,             //Pelvis

        RThigh,             //Muslo Derecho
        RLeg,               //Gemelo Derecho
        LThigh,             //Muslo Izquierdo
        LLeg,               //Gemelo Izquierdo

        RUpperArm,          //Antebrazo Derecho
        RForeArm,           //Brazo Derecho
        LUpperArm,          //Antebrazo Izquierdo
        LForeArm,           //Brazo Izquierdo
    }

    void Awake()
    {
        if(_animator == null) _animator = GetComponentInChildren<Animator>();
        if(_agent == null) _agent = GetComponent<NavMeshAgent>();
        if(_railController == null) _railController = _player.GetComponent<RailShooterController>();
        if(_animEvent == null) _animEvent = GetComponentInChildren<AnimEventController>();

        _bodyParts = new Dictionary<EnemyPart, Collider>();
    }

    public abstract void UpdateState();                           //Condiciones para cada estado del State Machine

    public abstract void Attack();

    public virtual void Shoot() { }

    public abstract void Death();

    public virtual void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public bool CanDie()
    {
        if (_life <= 0) return true;
        else return false;
    }

    public virtual void CheckParts()
    {
        //int quantity = 3;
    }

    public virtual void DoDamage(EnemyPart bodyPart, RaycastHit hit)
    {
        switch(bodyPart)
        {
            case EnemyPart.Head:
                _life -= 10;
                DeleteBodyPart(bodyPart);

                break;
            case EnemyPart.Chest: case EnemyPart.Abs:
                _life -= 2;
                if (_life < 4) DeleteBodyPart(bodyPart);

                break;
            case EnemyPart.Pelvis:
                _life -= 2;

                break;
            case EnemyPart.RThigh: case EnemyPart.RLeg: case EnemyPart.LThigh: case EnemyPart.LLeg:
                _life -= 2;

                break;
            case EnemyPart.RUpperArm:
                _life -= 2;
                if (_life < 8) DeleteBodyPart(EnemyPart.LForeArm);

                break;
            case EnemyPart.LUpperArm:
                _life -= 2;
                if (_life < 8) DeleteBodyPart(EnemyPart.LForeArm);

                break;
            case EnemyPart.RForeArm:
                _life -= 2;
                DeleteBodyPart(bodyPart);

                break;
            case EnemyPart.LForeArm:
                _life -= 2;
                DeleteBodyPart(bodyPart);

                break;
        }

        HitEffect(hit);

        /*
        if(_bodyParts.ContainsKey(bodyPart))
        {
            DeleteBodyPart(bodyPart);
        }
        */
    }

    public virtual void DeleteBodyPart(EnemyPart bodyPart)
    {
        Transform part = _bodyParts[bodyPart].gameObject.transform;
        ParticleSystem blood = Instantiate(_bloodParticle, part.position, Quaternion.identity);
        blood.transform.SetParent(part.parent);
        blood.transform.forward = part.up;
        blood.transform.localScale = new Vector3(.15f, .15f, .15f);
        blood.Play();

        _bodyParts[bodyPart].gameObject.transform.localScale = Vector3.zero;
        _bodyParts.Remove(bodyPart);
    }

    public virtual void HitEffect(RaycastHit hit)
    {
        ParticleSystem blood = Instantiate(_bloodImpact, hit.point, Quaternion.identity);
        blood.transform.forward = hit.normal;
        blood.Play();
    }

    public void SetNodeController(NodeController newNodeController)
    {
        _nodeController = newNodeController;
    }

}
