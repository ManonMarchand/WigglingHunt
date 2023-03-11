using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Tooltip("Speed of the player")]
        public float Speed;

        public AnimationCurve BoostCurve;
        public float DeviationLimit;

        public float TimeBeforeBoost;
        public float BoostDuration;
        public float MaxBoostSpeed;
    }
}