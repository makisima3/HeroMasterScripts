using System.Collections;
using UnityEngine;

namespace Assets.Code.AbilitySettings
{
    [System.Serializable]
    public class LaserAbilitySettings
    {
        [SerializeField] private float rayTickDelay;

        [SerializeField]  private float pushForceX;
        [SerializeField] private float pushForceY;

        [SerializeField] private float damageToEnemyNormal;
        [SerializeField] private float damageToEnemyStrong;
        [SerializeField] private float objectPushForceX;
        [SerializeField] private float objectPushForceY;

        public Vector3 Target { get; set; }
        public Vector3 PushDirection { get; set; }

        public float PushForceX => pushForceX;
        public float PushForceY => pushForceY;
        public float RayTickDelay => rayTickDelay;

        public float ObjectPushForceX => objectPushForceX;
        public float ObjectPushForceY => objectPushForceY;
        public float DamageToEnemyNormal => damageToEnemyNormal;
        public float DamageToEnemyStrong => damageToEnemyStrong;

    }
}