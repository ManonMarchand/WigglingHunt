using ScientificGameJam.Player;
using ScientificGameJam.SFX;
using UnityEngine;

namespace ScientificGameJam.Level
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField]
        private ColorType _color;

        private bool _registered = false;

        private void Start()
        {
            PlayerManager.Instance.RegisterCollectible(_color);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player") && collision.collider.attachedRigidbody.GetComponent<PlayerController>().Color == _color && !_registered)
            {
                _registered = true;
                PlayerManager.Instance.Collect(_color);
                if (SFXManager.Instance != null)
                    SFXManager.Instance.EatingSFX.Play();
                Destroy(gameObject);
            }
        }
    }
}
