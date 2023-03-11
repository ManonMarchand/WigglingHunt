using ScientificGameJam.Player;
using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Tooltip("Speed of the player")]
        public float Speed;
        public float Mass;

        public AnimationCurve BoostCurve;
        public float DeviationLimit;

        public float TimeBeforeBoost;
        public float BoostDuration;
        public float MaxBoostSpeed;

        public ColorType Color;

        public bool CanShoot;
    }
}