using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Player
{
    public class PlayerSpawn
    {
        public PlayerSpawn(Transform spawn, Color color)
        {
            Spawn = spawn;
            Color = color;
        }

        public bool DoesContainsPlayer(PlayerInput p)
            => Player != null && Player.GetInstanceID() == p.GetInstanceID();

        public override bool Equals(object obj)
        {
            return obj is PlayerSpawn ps && ps == this;
        }

        public override int GetHashCode()
        {
            return Player.GetInstanceID();
        }

        public static bool operator ==(PlayerSpawn p1, PlayerSpawn p2)
        {
            if (p1 is null)
            {
                return p2 is null;
            }
            if (p2 is null)
            {
                return false;
            }
            if (p1.Player == null)
            {
                return p2.Player == null;
            }
            if (p2.Player == null)
            {
                return false;
            }
            return p1.Player.GetInstanceID() == p2.Player.GetInstanceID();
        }

        public static bool operator !=(PlayerSpawn p1, PlayerSpawn p2)
            => !(p1 == p2);

        public PlayerInput Player { set; get; }
        public Transform Spawn { private set; get; }
        public Color Color { private set; get; }
    }
}
