using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] public bool _activateEnemies;
    [SerializeField] public List<GameObject> _enemyList;
    [SerializeField] public GameObject _player;
    [SerializeField] public RailShooterController _railController;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, 1f);
    }

    public void Start()
    {
        if(_activateEnemies)
        {
            foreach(var enemy in _enemyList)
            {
                enemy.GetComponent<Enemy>().SetNodeController(this);
            }
        }
    }

    public void Update()
    {
        if(_activateEnemies && _enemyList.Count == 0)
        {
            DeactivateEvent();
        }    
    }

    public void ActivateEvent()
    {
        //Debug.Log("ENEMY SPAWN");
        _player.GetComponent<Player>()._movement.SetTarget(_player.transform.position);

        foreach (GameObject enemy in _enemyList)
        {
            enemy.SetActive(true);
        }
    }

    public void DeactivateEvent()
    {
        //Debug.Log("DEACTIVATE EVENT");
        _railController._duringEvent = false;
        _activateEnemies = false;
        _railController.SetDefaultCamera();
    }

    public bool IsEnemyTrigger()
    {
        return _activateEnemies;
    }
}
