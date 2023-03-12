using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScientificGameJam.Menu
{
    public class LoadLevel : MonoBehaviour
    {
        [SerializeField]
        private string _level;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                SceneManager.LoadScene(_level);
            }
        }
    }
}
