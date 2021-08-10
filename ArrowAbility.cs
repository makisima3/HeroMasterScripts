using Assets.Code.AbilitySettings;
using Assets.Code.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.AbilityImpls
{
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowAbility : Ability<ArrowAbilitySettings>
    {
        [SerializeField] private GameObject arrowPrefab;

        private Rigidbody rigidbody;
        private Collider collider;

        private ArrowAbilitySettings settings;

        private Quaternion startRotation;

        private bool isDestoyed = false;

        private Vector3 direction;

        private bool isCollided = false;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }

        public override void Activate(ArrowAbilitySettings settings)
        {
            this.settings = settings;

            transform.LookAt(settings.Target);

            startRotation = transform.rotation;

            direction = GetDirection(transform.position, settings.Target);

            rigidbody.AddForce(direction * settings.FlyForce, ForceMode.Impulse);

            Destroy(gameObject, 5f);
        }

        private Vector3 GetDirection(Vector3 self, Vector3 target)
        {
            return (target - self).normalized;
        }

        public override void Deactivate(ArrowAbilitySettings settings)
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isDestoyed)
                return;

            if (isCollided)
                return;

            isCollided = true;

            //Debug.Log($"Arrow collide with: {collision.gameObject.name}");
            var arrowHandler = collision.gameObject.GetComponentInParent<IArrowHandler>();

            var contact = collision.GetContact(0);

            var arrow = Instantiate(arrowPrefab, contact.point, Quaternion.identity, collision.transform).transform;
            //arrow.rotation = Quaternion.LookRotation(-contact.normal, arrow.up);
            arrow.rotation = startRotation;

            if (arrowHandler != null)
            {
                settings.HitLayer = collision.gameObject.layer;
                //Толчок по напровлению полету пули+ немного вверх что бы враг не спытакался ногами об пол
                if (arrowHandler is BodyPart)
                    settings.PushDirection = direction * settings.PushForceX + Vector3.up * settings.PushForceY;
                else if (arrowHandler is ShootableObject)
                    settings.PushDirection = direction * settings.ObjectPushForceX + Vector3.up * settings.ObjectPushForceY;

                arrowHandler.Hadle(settings);
            }

            isDestoyed = true;
            Destroy(gameObject);
        }
    }
}