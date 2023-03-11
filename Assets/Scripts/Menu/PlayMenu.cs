using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScientificGameJam.Menu
{
    public class PlayMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
