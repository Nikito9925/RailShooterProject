using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    [Header("Skybox")]
    [SerializeField] private float _rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        if(_player._life <= 0)
        {
            SceneManager.LoadScene("DefeatScreen");
        }

        RenderSettings.skybox.SetFloat("_Rotation", RenderSettings.skybox.GetFloat("_Rotation") + (Time.deltaTime * _rotationSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("VictoryScreen");
        }
    }

}
