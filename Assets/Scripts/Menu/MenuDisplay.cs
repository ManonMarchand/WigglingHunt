using ScientificGameJam.Translation;
using System;
using UnityEngine;

namespace ScientificGameJam.Menu
{
    public class MenuDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject _credits;

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

                case "english": Translate.Instance.CurrentLanguage = "english"; break;
                case "french": Translate.Instance.CurrentLanguage = "french"; break;

                default: throw new NotImplementedException();
            }
        }
    }
}
