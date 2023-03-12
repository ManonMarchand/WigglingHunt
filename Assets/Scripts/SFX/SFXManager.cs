using UnityEngine;

namespace ScientificGameJam.SFX
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public AudioSource LaserSFX, WinningSFX, LoosingSFX, TeleportSFX, EatingSFX, BGM;
    }
}
