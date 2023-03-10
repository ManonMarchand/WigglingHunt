using UnityEngine;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Player
{
    public class PlayerSpawn
    {
        public PlayerSpawn(Transform spawn)
        {
            Spawn = spawn;
        }

        public bool DoesContainsPlayer(PlayerInput p)
            => Player.GetInstanceID() == p.GetInstanceID();

        public PlayerInput Player { set; get; }
        public Transform Spawn { private set; get; }
    }
}
