using System;
using System.Linq;
using TMPro;
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

        [SerializeField]
        private TMP_Text _waitingPlayerText;

        private PlayerSpawn[] _spawns;

        private bool _isReady;
        public bool IsReady
        {
            set
            {
                _waitingPlayerText.gameObject.SetActive(!value);
                _isReady = value;
            }
            get => _isReady;
        }

        private void Awake()
        {
            Instance = this;
            _spawns = _spawnPoint.Select(x => new PlayerSpawn(x)).ToArray();
        }

        public PlayerInput GetNextPlayer(PlayerInput player)
        {
            return _spawns.Select(x => x.Player).FirstOrDefault(x => x != null && x != player);
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
                if (_spawns.All(x => x.Player != null))
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
