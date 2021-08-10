using Assets.Code.AbilitySettings;
using Assets.Code.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Entities.AbilityImpls
{
    [RequireComponent(typeof(Rigidbody))]
    public class MagicWaveAbility : Ability<MagicWaveAbilitySettings>
    {
        private Rigidbody rigidbody;
        private Collider collider;

        private MagicWaveAbilitySettings settings;

        private List<IMagicWaveHandler> hitedEnemies;

        private Vector3 direction;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }

        public override void Activate(MagicWaveAbilitySettings settings)
        {
            this.settings = settings;

            transform.LookAt(settings.Target);

            hitedEnemies = new List<IMagicWaveHandler>();

            direction = GetDirection(transform.position, settings.Target);

            rigidbody.AddForce(direction * settings.FlySpeed, ForceMode.Impulse);

            Destroy(gameObject, 5f);
        }

        private Vector3 GetDirection(Vector3 self, Vector3 target)
        {
            return (target - self).normalized;
        }

        public override void Deactivate(MagicWaveAbilitySettings settings)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"Wave collide with: {other.gameObject.name}");
            var magicWaveHandler = other.gameObject.GetComponentInParent<IMagicWaveHandler>();

            //var contact = collision.GetContact(0);

            //var arrow = Instantiate(arrowPrefab, contact.point, Quaternion.identity, collision.transform).transform;
            ////arrow.rotation = Quaternion.LookRotation(-contact.normal, arrow.up);
            //arrow.rotation = startRotation;

            if (magicWaveHandler == null)
                return;

            if (hitedEnemies.Contains(magicWaveHandler))
                return;

            if (magicWaveHandler is BodyPart)
                settings.PushDirection = direction * settings.PushForceX + Vector3.up * settings.PushForceY;
            else if (magicWaveHandler is ShootableObject)
                settings.PushDirection = direction * settings.ObjectPushForceX + Vector3.up * settings.ObjectPushForceY;

            magicWaveHandler.Hadle(settings);

            hitedEnemies.Add(magicWaveHandler);
        }
    }
}