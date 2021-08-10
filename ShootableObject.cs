using Assets.Code.AbilitySettings;
using Assets.Code.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities
{
    [RequireComponent(typeof(Rigidbody),typeof(Collider))]
    public class ShootableObject : MonoBehaviour, IWebHandler, IArrowHandler, ILaserHandler, IMagicWaveHandler
    {
        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Hadle(WebAbilitySettings settings)
        {
            rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);
        }

        public void Hadle(ArrowAbilitySettings settings)
        {
            rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);
        }

        public void Hadle(LaserAbilitySettings settings)
        {
            rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);
        }

        public void Hadle(MagicWaveAbilitySettings settings)
        {
            rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);
        }
    }
}