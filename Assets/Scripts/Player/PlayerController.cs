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
        private Camera _cam;

        // Movement vector
        private Vector2 _mov;
        // Movement vector on previous physic frame
        private Vector2 _prevMov;

        // Internal timer to calculate boost
        private float _boostTimer;

        private Vector2 _aimDir;

        public ColorType Color => Info.Color;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _cam = GetComponentInChildren<Camera>();
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

        public void OnAim(InputAction.CallbackContext value)
        {
            var v2 = value.ReadValue<Vector2>();
            if (_input.currentControlScheme == "Keyboard&Mouse")
            {
                v2 = _cam.ScreenToWorldPoint(v2);
                _aimDir = new(v2.x - transform.position.x, v2.y - transform.position.y);
            }
            else
            {
                if (v2 != Vector2.zero)
                {
                    _aimDir = new(v2.x, v2.y);
                    Debug.Log(_aimDir);
                }
            }
        }

        public void OnFire(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                var hit = Physics2D.Raycast(transform.position, _aimDir, float.PositiveInfinity, ~(1 << 6));
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                }
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawRay(new(transform.position, _aimDir));
        }
    }
}
