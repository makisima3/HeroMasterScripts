using System.Collections;
using UnityEngine;

namespace Assets.Code.AbilitySettings
{
    [System.Serializable]
    public class WebAbilitySettings
    {
        [SerializeField] private float flyForce;

        [SerializeField] private float pushForceX;
        [SerializeField] private float pushForceY;
        [SerializeField] private float objectPushForceX;
        [SerializeField] private float objectPushForceY;

        public Vector3 Target { get; set; }
        public Vector3 PushDirection { get; set; }
        public Transform WebViwe { get; set; }

        public float FlyForce => flyForce;
        public float PushForceX => pushForceX;
        public float PushForceY => pushForceY;

        public float ObjectPushForceX => objectPushForceX;
        public float ObjectPushForceY => objectPushForceY;
    }
}