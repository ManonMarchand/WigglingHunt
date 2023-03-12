using System.Collections;
using UnityEngine;

namespace ScientificGameJam.Level
{
    public class Wiggle : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [SerializeField]
        private float _maxTorque;

        [SerializeField]
        private float _maxDistance;
        [SerializeField]
        private float _angularDispersion;
        private bool _motionAllowed = true;


        private Vector2 _mov;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
        }
        private IEnumerator TimeTillMotion()
        {
            _motionAllowed = false;
            yield return new WaitForSeconds(.2f);
            _motionAllowed = true;
        }

        private void Update()
        {
            if (_motionAllowed)
            {
                float randDistanceX = Random.Range(-_maxDistance, _maxDistance);
                float randDistanceY = Random.Range(-_maxDistance, _maxDistance);
                var randVectorDistance = new Vector2(randDistanceX, randDistanceY);
                float randAngle = Random.Range(-_angularDispersion, _angularDispersion);
                if (Mathf.Abs(_rb.totalTorque) < _maxTorque)
                {
                    _rb.AddTorque(randAngle, ForceMode2D.Force);
                }
                
                _rb.AddForce(randVectorDistance, ForceMode2D.Impulse);

            }
            StartCoroutine(TimeTillMotion());

            // Debug.Log($"Velocity {_rb.velocity.magnitude}");
        }
    }
}
