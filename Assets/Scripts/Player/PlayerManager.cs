using ScientificGameJam.SO;
using ScientificGameJam.Translation;
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
        [SerializeField]
        private PlayerInfo[] _infos;

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

        [SerializeField]
        private Camera _waitingCamera;

        [SerializeField]
        private GameObject _separator;

        private readonly List<PlayerSpawn> _spawns = new();

        private Dictionary<ColorType, int> _remainingCollectibles = new();

        public void RegisterCollectible(ColorType color)
        {
            if (!_remainingCollectibles.ContainsKey(color))
            {
                _remainingCollectibles.Add(color, 1);
            }
            else
            {
                _remainingCollectibles[color]++;
            }
        }

        public int GetCollectibleLeft(ColorType color)
        {
            return _remainingCollectibles[color];
        }

        public void Collect(ColorType color)
        {
            _remainingCollectibles[color]--;
            CheckGlobalVictory();
            foreach (var s in _spawns)
            {
                if (s.Player != null)
                {
                    s.Player.GetComponent<PlayerController>().UpdateDyeText();
                }
            }
        }

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
            _spawns.Add(new(spawn, _infos[_spawns.Count % 2]));
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
            if (_spawns.All(x => x.IsWinning) && _remainingCollectibles.Values.All(x => x == 0))
            {
                Debug.Log(Translate.Instance.Tr("winningText"));
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
                player.GetComponent<PlayerController>().Info = _spawns[freeSpot].Info;
                player.GetComponent<Rigidbody2D>().mass = _spawns[freeSpot].Info.Mass;
                player.GetComponent<SpriteRenderer>().color = ToColor(_spawns[freeSpot].Info.Color);
                _waitingCamera.gameObject.SetActive(false);
                _separator.SetActive(_spawns.Count(x => x.Player != null) > 1);
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
            _separator.SetActive(_spawns.Count(x => x.Player != null) > 1);
            if (_spawns.All(x => x.Player == null))
            {
                _waitingCamera.gameObject.SetActive(true);
            }
        }
    }
}
