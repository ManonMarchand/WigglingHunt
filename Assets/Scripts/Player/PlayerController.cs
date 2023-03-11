using ScientificGameJam.SFX;
using ScientificGameJam.SO;
using ScientificGameJam.Translation;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ScientificGameJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInfo Info { set; private get; }

        [SerializeField]
        private TMP_Text _dyeLeftText;

        private Rigidbody2D _rb;
        private PlayerInput _input;
        private Camera _cam;
        private LineRenderer _lr;

        private float _laserTimer;

        private bool _canShoot = true;

        // Movement vector
        private Vector2 _mov;
        // Movement vector on previous physic frame
        private Vector2 _prevMov;

        // Internal timer to calculate boost
        private float _boostTimer;

        private Vector2 _aimDir;

        public ColorType Color => Info.Color;

        private int _ignoreMask;

        private float _shake;

        private float _decreaseFactor=0.7f;

        private void Shake()
        {
            if (_shake > 0f)
            {
                _cam.transform.localPosition = UnityEngine.Random.insideUnitCircle * Info.ShakeAmount;
                _cam.transform.localPosition = new(_cam.transform.localPosition.x, _cam.transform.localPosition.y, -10f);
                _shake -= Time.deltaTime * _decreaseFactor;

            }
            else
            {
                _shake = 0f;
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            _cam = GetComponentInChildren<Camera>();
            _lr = GetComponentInChildren<LineRenderer>();
            _lr.gameObject.SetActive(false);

            _ignoreMask = (1 << LayerMask.NameToLayer("Player"));
            _ignoreMask |= (1 << LayerMask.NameToLayer("Collectible"));
            _ignoreMask = ~_ignoreMask;

            UpdateDyeText();
        }

        private bool CanPlay => PlayerManager.Instance.IsReady && !PlayerManager.Instance.DidGameEnded;

        private void FixedUpdate()
        {
            if (CanPlay)
            {
                // Debug.Log($"Dot product value {Vector2.Dot(_prevMov, _mov)}");
                if (Vector2.Dot(_prevMov, _mov) < Info.DeviationLimit) // condition on loosing booster
                {
                    //Debug.Log("I did a reset");
                    _boostTimer = 0f; // in seconds
                }
                

                _prevMov = _mov;
                _rb.velocity = Info.Speed * Time.fixedDeltaTime * _mov * (_boostTimer >= Info.TimeBeforeBoost ? Info.Booster * ( 1f+ Info.BoostCurve.Evaluate(Time.fixedDeltaTime)) : 1f);
                // Debug.Log($"Velocity {_rb.velocity.magnitude}");
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }

        private void Update()
        {
            _boostTimer += Time.deltaTime;
            if (_laserTimer > 0f)
            {
                _laserTimer -= Time.deltaTime;
                if (_laserTimer <= 0f)
                {
                    _lr.gameObject.SetActive(false);
                }
            }
            Shake();
        }

        public void UpdateDyeText()
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                _dyeLeftText.text = $"{Translate.Instance.Tr("dyeLeft")} {PlayerManager.Instance.GetCollectibleLeft(Info.Color)}";
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                PlayerManager.Instance.GameOver();
            }
        }

        public void Move(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;
        }

        public void OnTeleport(InputAction.CallbackContext value)
        {
            if (value.performed && CanPlay)
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
                }
            }
        }

        private Vector3 ConvertVector(Vector3 value)
            => new(value.x, value.y, -1f);

        public void OnFire(InputAction.CallbackContext value)
        {
            if (value.performed && Info.CanShoot && _canShoot && CanPlay)
            {
                var hit = Physics2D.Raycast(transform.position, _aimDir, float.PositiveInfinity, _ignoreMask);
                if (hit.collider != null)
                {
                    SFXManager.Instance.LaserSFX.Play();
                    _shake = Info.ShakeTime;
                    _lr.gameObject.SetActive(true);
                    _lr.SetPositions(new[] { ConvertVector(transform.position), ConvertVector((Vector3)hit.point) });
                    _laserTimer = .3f;
                    if (hit.collider.TryGetComponent<Rigidbody2D>(out var comp))
                    {
                        comp.AddForce(_aimDir.normalized * 10f, ForceMode2D.Impulse);
                    }
                    _canShoot = false;
                    StartCoroutine(Reload());
                }
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(Info.LaserReloadTime);
            _canShoot = true;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawRay(new(transform.position, _aimDir));
        }
    }
}
