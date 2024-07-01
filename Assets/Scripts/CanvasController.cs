using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _bulletBar;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxBullets;

    public void UpdateCanvas(int newHealth, int newBullets)
    {
        _healthBar.fillAmount = (float)(newHealth) / _maxHealth;
        _bulletBar.fillAmount = (float)(newBullets) / _maxBullets;
    }
}
