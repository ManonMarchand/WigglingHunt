using UnityEngine;

namespace ScientificGameJam.Player
{
    public class RegisterSpawn : MonoBehaviour
    {
        private void Start()
        {
            PlayerManager.Instance.RegisterSpawn(transform);
        }
    }
}
