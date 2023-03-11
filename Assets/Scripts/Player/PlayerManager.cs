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
        public static Color ToColor(ColorType type)
        {
            return type switch
            {
                ColorType.RED => Color.red,
                ColorType.GREEN => Color.green,
                _ => throw new NotImplementedException()
            };
        }

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

        private static ColorType[] _colors = new[] { ColorType.RED, ColorType.GREEN };
        public void RegisterSpawn(Transform spawn)
        {
            _spawns.Add(new(spawn, _colors[_spawns.Count % 2]));
        }

        public PlayerSpawn GetSpawn(PlayerInput player)
        {
            return _spawns.FirstOrDefault(x => x.DoesContainsPlayer(player));
        }

        public PlayerInput GetNextPlayer(PlayerInput player)
        {
            return _spawns.Select(x => x.Player).FirstOrDefault(x => x != null && x != player);
        }

        public void CheckGlobalVictory()
        {
            if (_spawns.All(x => x.IsWinning))
            {
                Debug.Log("Wow you won");
            }
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
                var c = _spawns[freeSpot].Color;
                player.GetComponent<SpriteRenderer>().color = ToColor(c);
                player.GetComponent<PlayerController>().Color = c;
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
