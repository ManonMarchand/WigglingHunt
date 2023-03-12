using ScientificGameJam.Translation;
using TMPro;
using UnityEngine;

namespace ScientificGameJam.Menu
{
    public class MenuDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject _credits, _controls, _explanations;
        [SerializeField]
        private TMP_Text _explanationsTitle, _explanationsContent;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("InfoPanel"))
            {
                Toggle(collision.GetComponent<InfoPanel>().Tag, true);
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("InfoPanel"))
            {
                Toggle(collision.GetComponent<InfoPanel>().Tag, false);
            }
        }

        private void Toggle(string tag, bool value)
        {
            switch (tag)
            {
                case "Credits": _credits.SetActive(value); break;
                case "Controls": _controls.SetActive(value); break;

                case "english": if (value) Translate.Instance.CurrentLanguage = "english"; break;
                case "french": if (value) Translate.Instance.CurrentLanguage = "french"; break;

                default:
                    _explanations.SetActive(value);
                    _explanationsTitle.text = "";
                    _explanationsContent.text = "";
                    break;
            }
        }
    }
}
