using System.Collections;
using UnityEngine;

public class AnimEventController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    public void Shoot()
    {
        _enemy.Shoot();
    }

    public void StartDeath()
    {
        StartCoroutine(Death());
    }

    public void DoDamage()
    {
        _enemy._player.GetComponent<Player>().DoDamage();
    }

    public IEnumerator Death()
    {
        _enemy._bloodPuddle.SetActive(true);
        _enemy._agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        yield return new WaitForSeconds(2);

        _enemy._bloodPuddle.GetComponent<Animator>().SetTrigger("Despawn");
        _enemy._bloodPuddle.transform.SetParent(null);
        Destroy(_enemy.gameObject);

    }
}
