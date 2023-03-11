using ScientificGameJam.Player;
using UnityEngine;

namespace ScientificGameJam.Level
{
    public class EnemyController : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                PlayerManager.Instance.GameOver(false);
            }
        }
    }
}
