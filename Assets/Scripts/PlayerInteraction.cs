using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int emberPoints = 10;
    [SerializeField] private int goalBonus = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ember"))
        {
            other.gameObject.SetActive(false);
            gameManager.AddScore(emberPoints);
        }
        else if (other.CompareTag("Hazard") || other.CompareTag("Enemy"))
        {
            gameManager.EndGame(false);
        }
        else if (other.CompareTag("Goal"))
        {
            gameManager.AddScore(goalBonus);
            gameManager.EndGame(true);
        }
    }
}
