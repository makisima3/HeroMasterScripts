using Assets.Code.AbilitySettings;
using Assets.Code.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Enemy : MonoBehaviour, IArrowHandler, IMagicWaveHandler, IWebHandler, ILaserHandler
    {
        public Waypoint MyWaypoint { get; set; }

        public abstract Rigidbody Rigidbody { get; }

        public abstract List<BodyPart> BodyParts { get; }

        public abstract float HP { get; }

        public abstract void Hadle(WebAbilitySettings settings);

        public abstract void Hadle(ArrowAbilitySettings settings);

        public abstract void Hadle(MagicWaveAbilitySettings settings);

        public abstract void Hadle(LaserAbilitySettings settings);

        public abstract void Attack(Vector3 playerPosition);

        public abstract void Hit(float damage);

        public abstract void Stop();
    }
}