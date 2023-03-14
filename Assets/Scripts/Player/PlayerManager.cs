using ScientificGameJam.SFX;
using ScientificGameJam.SO;
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

        private float _timeRef;

        public static Color ToColor(ColorType type)
        {
            return type switch
            {
                ColorType.GREEN => new Color(156f / 255f, 192f / 255f, 156f / 255f),
                ColorType.RED => new Color(199f / 255f, 154f / 255f, 149f / 255f),
                _ => throw new NotImplementedException()
            };
        }

        public static PlayerManager Instance { get; private set; }

        [SerializeField]
        private TMP_Text _waitingPlayerText, _timerText;

        [SerializeField]
        private Camera _waitingCamera;

        [SerializeField]
        private GameObject _separator;

        [SerializeField]
        private GameObject _victory, _gameover, _reasonTouching;

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
            if (!_remainingCollectibles.ContainsKey(color))
            {
                return 0;
            }
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
                try
                {
                    _waitingPlayerText.gameObject.SetActive(!value);
                }
                catch (Exception _)
                {
                    // TODO
                }
                _isReady = value;
            }
            get => _isReady;
        }

        public bool DidGameEnded { private set; get; }

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
                _timerText.text = $"{Time:0.00}s";
                SFXManager.Instance.BGM.Stop();
                SFXManager.Instance.WinningSFX.Play();
                DidGameEnded = true;
                _victory.SetActive(true);
            }
        }

        public void GameOver(bool didTouch)
        {
            SFXManager.Instance.BGM.Stop();
            SFXManager.Instance.LoosingSFX.Play();
            DidGameEnded = true;
            _gameover.SetActive(true);
            if (didTouch)
            {
                _reasonTouching.SetActive(true);
            }
        }

        private float Time => UnityEngine.Time.unscaledTime - _timeRef;

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
                player.GetComponentInChildren<SpriteRenderer>().transform.localScale = new(_spawns[freeSpot].Info.Scale, _spawns[freeSpot].Info.Scale, 1f);
                player.GetComponent<PlayerController>().Info = _spawns[freeSpot].Info;
                player.GetComponent<Rigidbody2D>().mass = _spawns[freeSpot].Info.Mass;
                player.GetComponentInChildren<Animator>().runtimeAnimatorController = _spawns[freeSpot].Info.Anim;

                var layer = LayerMask.NameToLayer(_spawns[freeSpot].Info.Color == ColorType.RED ? "RedPlayer" : "GreenPlayer");
                player.gameObject.layer = layer;
                player.GetComponentInChildren<SpriteRenderer>().gameObject.layer = layer;

                _waitingCamera.gameObject.SetActive(false);
                _separator.SetActive(_spawns.Count(x => x.Player != null) > 1);
                if (_spawns.All(x => x.Player != null))
                {
                    IsReady = true;
                    _timeRef = UnityEngine.Time.unscaledTime;
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
