using UnityEngine;

namespace Assets.Scripts.Player
{
    public class SelectRandomChild : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _sprites;

        private void Awake()
        {
            _sprites[Random.Range(0, _sprites.Length)].SetActive(true);
        }
    }
}
