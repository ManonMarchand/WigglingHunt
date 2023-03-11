using System;
using System.Collections.Generic;
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
        private TMP_Text _waitingPlayerText;

        private readonly List<PlayerSpawn> _spawns = new();

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
        }

        public void RegisterSpawn(Transform spawn)
        {
            _spawns.Add(new(spawn));
        }

        public PlayerInput GetNextPlayer(PlayerInput player)
        {
            return _spawns.Select(x => x.Player).FirstOrDefault(x => x != null && x != player);
        }

        public void OnPlayerJoin(PlayerInput player)
        {
            var freeSpot = _spawns.IndexOf(_spawns.FirstOrDefault(x => x.Player == null));
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
            var freeSpot = _spawns.IndexOf(_spawns.FirstOrDefault(x => x.DoesContainsPlayer(player)));
            Assert.AreNotEqual(-1, freeSpot);
            _spawns[freeSpot].Player = null;
            IsReady = false;
        }
    }
}
