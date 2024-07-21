using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    // Update is called once per frame
    void Update()
    {
        if(_player._life <= 0)
        {
            SceneManager.LoadScene("DefeatScreen");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("VictoryScreen");
        }
    }

}
