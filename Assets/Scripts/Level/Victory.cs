using ScientificGameJam.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Level
{
    public class Victory : MonoBehaviour
    {
        [SerializeField]
        private ColorType _targetColor;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var spawn = PlayerManager.Instance.GetSpawn(collision.attachedRigidbody.GetComponent<PlayerInput>());
                if (spawn.Color == _targetColor)
                {
                    spawn.IsWinning = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var spawn = PlayerManager.Instance.GetSpawn(collision.GetComponent<PlayerInput>());
                if (spawn.Color == _targetColor)
                {
                    spawn.IsWinning = false;
                }
            }
        }
    }
}
