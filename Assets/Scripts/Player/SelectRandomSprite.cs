using UnityEngine;

namespace Assets.Scripts.Player
{
    public class SelectRandomSprite : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _sprites;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
        }
    }
}
