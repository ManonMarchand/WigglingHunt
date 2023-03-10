using ScientificGameJam.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spawnPoint;

        private PlayerSpawn[] _spawns;

        private void Awake()
        {
            _spawns = _spawnPoint.Select(x => new PlayerSpawn(x)).ToArray();
        }

        public void OnPlayerJoin(PlayerInput player)
        {
            var freeSpot = Array.IndexOf(_spawns, _spawns.FirstOrDefault(x => x.Player == null));
            Assert.AreNotEqual(-1, freeSpot);
            _spawns[freeSpot].Player = player;
            player.transform.position = _spawns[freeSpot].Spawn.position;
        }

        public void OnPlayerLeave(PlayerInput player)
        {
            var freeSpot = Array.IndexOf(_spawns, _spawns.Select(x => x.DoesContainsPlayer(player)));
            Assert.AreNotEqual(-1, freeSpot);
            _spawns[freeSpot].Player = null;
        }
    }
}
