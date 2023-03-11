using ScientificGameJam.Player;
using UnityEngine;

namespace ScientificGameJam.Level
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField]
        private ColorType _color;

        private void Start()
        {
            PlayerManager.Instance.RegisterCollectible(_color);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && collision.GetComponent<PlayerController>().Color == _color)
            {
                Debug.Log(collision.GetComponent<PlayerController>().Color);
                Debug.Log(_color);
                PlayerManager.Instance.Collect(_color);
                Destroy(gameObject);
            }
        }
    }
}
