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
        public static PlayerManager Instance { get; private set; }

        [SerializeField]
        private Transform[] _spawnPoint;

        private PlayerSpawn[] _spawns;

        public bool IsReady { get; private set; }

        private void Awake()
        {
            Instance = this;
            _spawns = _spawnPoint.Select(x => new PlayerSpawn(x)).ToArray();
        }

        public void OnPlayerJoin(PlayerInput player)
        {
            var freeSpot = Array.IndexOf(_spawns, _spawns.FirstOrDefault(x => x.Player == null));
            if (freeSpot == -1)
            {
                Debug.LogWarning("No spot found for the new input device!");
                Destroy(player.gameObject);
            }
            else
            {
                _spawns[freeSpot].Player = player;
                player.transform.position = _spawns[freeSpot].Spawn.position;
                if (_spawns.All(x => x != null))
                {
                    IsReady = true;
                }
            }
        }

        public void OnPlayerLeave(PlayerInput player)
        {
            var freeSpot = Array.IndexOf(_spawns, _spawns.FirstOrDefault(x => x.DoesContainsPlayer(player)));
            Assert.AreNotEqual(-1, freeSpot);
            _spawns[freeSpot].Player = null;
            IsReady = false;
        }
    }
}
