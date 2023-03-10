using ScientificGameJam.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private Rigidbody2D _rb;
        private PlayerInput _input;
        private Vector2 _mov;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            if (PlayerManager.Instance.IsReady)
            {
                _rb.velocity = _info.Speed * Time.fixedDeltaTime * _mov;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                // TODO: Victory screen
            }
        }

        public void Move(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;
        }

        public void OnTeleport(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                var next = PlayerManager.Instance.GetNextPlayer(_input);
                if (next != null) // Might happens if the others players aren't instanciated yet
                {
                    var tmpPos = transform.position;
                    transform.position = next.transform.position;
                    next.transform.position = tmpPos;
                }
            }
        }
    }
}
