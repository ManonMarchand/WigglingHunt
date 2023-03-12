using ScientificGameJam.Translation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScientificGameJam.Level
{
    public class DisplayWinText : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = Translate.Instance.Tr(SceneManager.GetActiveScene().name == "Level0" ? "winningLevel0" : "winningText");
        }
    }
}
