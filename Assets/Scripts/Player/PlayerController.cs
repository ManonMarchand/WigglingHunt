using ScientificGameJam.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInfo Info { set; private get; }

        private Rigidbody2D _rb;
        private PlayerInput _input;
        private Vector2 _mov;

        private Vector2 _prevMov;
        private float _boostTimer;

        public ColorType Color { set; get; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            if (PlayerManager.Instance.IsReady)

            {
                // Debug.Log($"Dot product value {Vector2.Dot(_prevMov, _mov)}");
                if (Vector2.Dot(_prevMov, _mov) < Info.DeviationLimit) // condition on loosing booster
                {
                    Debug.Log("I did a reset");
                    _boostTimer = 0f; // in seconds
                }

                _prevMov = _mov;
                _rb.velocity = Info.Speed * Time.fixedDeltaTime * _mov * (Info.TimeBeforeBoost > 0f && _boostTimer >= Info.TimeBeforeBoost ? 2f : 1f);
                // Debug.Log($"Velocity {_rb.velocity.magnitude}");
            }
        }

        private void Update()
        {
            _boostTimer += Time.deltaTime;
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
                    (next.transform.position, transform.position) = (transform.position, next.transform.position);
                }
            }
        }
    }
}
