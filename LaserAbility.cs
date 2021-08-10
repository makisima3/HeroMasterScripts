using Assets.Code.AbilitySettings;
using Assets.Code.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Entities.AbilityImpls
{
    [RequireComponent(typeof(Rigidbody))]
    public class LaserAbility : Ability<LaserAbilitySettings>
    {
        private Rigidbody rigidbody;
        private Collider collider;

        private LaserAbilitySettings settings;

        private bool isShooting = false;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }

        public override void Activate(LaserAbilitySettings settings)
        {
            gameObject.SetActive(true);

            this.settings = settings;

            isShooting = true;

            StartCoroutine(CollisionCheck());
        }

        public override void Deactivate(LaserAbilitySettings settings)
        {
            isShooting = false;

            gameObject.SetActive(false);
        }

        private IEnumerator CollisionCheck()
        {
            while(isShooting)
            {
                //Debug.Log($"LaserAbility target: {settings.Target}");

                Ray ray = new Ray(transform.position,(settings.Target - transform.position).normalized);

                if(Physics.Raycast(ray,out RaycastHit hit,1000))
                {
                    if(hit.transform.gameObject.TryGetComponent<ILaserHandler>(out var handler))
                    {
                        //Толчок по напровлению полету пули+ немного вверх что бы враг не спытакался ногами об пол
                        if (handler is BodyPart)
                            settings.PushDirection = ray.direction * settings.PushForceX + Vector3.up * settings.PushForceY;
                        else if (handler is ShootableObject)
                            settings.PushDirection = ray.direction * settings.ObjectPushForceX + Vector3.up * settings.ObjectPushForceY;

                        handler.Hadle(settings);
                    }
                }

                yield return new WaitForSeconds(settings.RayTickDelay);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log($"Wave collide with: {collision.gameObject.name}");
            //var laserHandler = collision.gameObject.GetComponentInParent<ILaserHandler>();


            //if (laserHandler != null)
            //{

            //    laserHandler.Hadle(settings);

            //    collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * settings.PushForce, ForceMode.Impulse);

            //}
        }
    }
}