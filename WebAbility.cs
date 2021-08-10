using Assets.Code.AbilitySettings;
using Assets.Code.Entities.Enemies;
using Assets.Code.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.AbilityImpls
{
    [RequireComponent(typeof(Rigidbody))]
    public class WebAbility : Ability<WebAbilitySettings>
    {
        [SerializeField] private GameObject webPrefab;

        private new Rigidbody rigidbody;

        private WebAbilitySettings settings;

        private Vector3 direction;

        private bool isCollided = false;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void Activate(WebAbilitySettings settings)
        {
            this.settings = settings;

            transform.LookAt(settings.Target);

            direction = GetDirection(transform.position, settings.Target);

            rigidbody.AddForce(direction * settings.FlyForce, ForceMode.Impulse);

            Destroy(gameObject, 5f);
        }

        private Vector3 GetDirection(Vector3 self, Vector3 target)
        {
            return (target - self).normalized;
        }

        public override void Deactivate(WebAbilitySettings settings)
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log($"Web collide with: {collision.gameObject.name}");

            if (isCollided)
                return;

            isCollided = true;

            

            var contact = collision.GetContact(0);
            if (collision.gameObject.GetComponentInParent<EnemyStrong>() == null)
            {
                var web = Instantiate(webPrefab, contact.point, Quaternion.identity, collision.transform).transform;
                web.rotation = Quaternion.LookRotation(-contact.normal, web.up);
                settings.WebViwe = web;
            }

            if (collision.gameObject.TryGetComponent<IWebHandler>(out var handler))
            {
                //Толчок по напровлению полету пули+ немного вверх что бы враг не спытакался ногами об пол
                if (handler is BodyPart)
                    settings.PushDirection = direction * settings.PushForceX + Vector3.up * settings.PushForceY;
                else if (handler is ShootableObject)
                    settings.PushDirection = direction * settings.ObjectPushForceX + Vector3.up * settings.ObjectPushForceY;
                handler.Hadle(settings);
                //webHandler.Rigidbody.AddForce(direction * settings.PushForceX + Vector3.up * settings.PushForceY, ForceMode.Impulse);
            }



            Destroy(gameObject);
        }
    }
}