using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScientificGameJam.Menu
{
    public class GameOver : MonoBehaviour
    {
        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
