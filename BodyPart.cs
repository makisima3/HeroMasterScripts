using Assets.Code.AbilitySettings;
using Assets.Code.Entities.AbilityImpls;
using Assets.Code.Entities.Enemies;
using Assets.Code.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.Entities
{
    public class BodyPart : MonoBehaviour, IWebHandler, ILaserHandler, IArrowHandler, IMagicWaveHandler
    {
        public IWebHandler ParentWebHandler { get; set; }
        public ILaserHandler ParentLaserHandler { get; set; }

        public IArrowHandler ParentArrowHandler { get; set; }
        public IMagicWaveHandler ParentMagicWaveHandler { get; set; }

        public Enemy ParentEnemy { get; set; }

        public bool IsShoted { get; set; }

        private bool IsCollided = false;

        private new Rigidbody rigidbody;
        private Collider collider;
        private WebAbilitySettings webAbilitySettings;

        public Rigidbody Rigidbody => rigidbody;
        public Collider Collider => collider;

        

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }

        public void Hadle(WebAbilitySettings settings)
        {
            //Debug.Log("enemyHandlWeb");
            IsShoted = true;
            ParentWebHandler.Hadle(settings);

            Rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);

            webAbilitySettings = settings;
        }

        public void Hadle(LaserAbilitySettings settings)
        {
            ParentLaserHandler.Hadle(settings);

            Rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);
        }

        public void Hadle(MagicWaveAbilitySettings settings)
        {
            var direction = settings.PushDirection;

            if (ParentMagicWaveHandler is EnemyStrong)
                direction *= settings.PushModifierForEnemyStrong;

            Rigidbody.AddForce(direction, ForceMode.Impulse);

            ParentMagicWaveHandler.Hadle(settings);
        }

        public void Hadle(ArrowAbilitySettings settings)
        {
            Rigidbody.AddForce(settings.PushDirection, ForceMode.Impulse);

            ParentArrowHandler.Hadle(settings);
        }


        //Столкновение с окружением 
        private void OnCollisionEnter(Collision collision)
        {
            if (!IsShoted)
                return;

            if (IsCollided)
                return;

            if (collision.gameObject.GetComponent<WebAbility>() == null
                && collision.gameObject.GetComponent<BodyPart>() == null)
            {
                Debug.Log($"BodyPArtName: {gameObject.name} collide with: {collision.gameObject.name}");

                var contact = collision.GetContact(0);
                webAbilitySettings.WebViwe.rotation = Quaternion.LookRotation(-contact.normal, webAbilitySettings.WebViwe.up);

                var joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                joint.enableCollision = false;

                ParentEnemy.Rigidbody.useGravity = false;
                
                ParentEnemy.BodyParts.ForEach(bp =>
                {
                    bp.PausePhysics(0.001f);
                });

                ParentEnemy.Hit(ParentEnemy.HP);

                IsCollided = true;
            }

        }

        public void PausePhysics(float delay)
        {
            StopPhysics();

            StartCoroutine(Delay(delay));
        }

        public void StopPhysics()
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Rigidbody.isKinematic = true;
            Rigidbody.velocity = Vector3.zero;
        }

        public void StartPhysics()
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
            Rigidbody.isKinematic = false;
        }

        private IEnumerator Delay(float delay)
        {
            yield return new WaitForSeconds(delay);

            ParentEnemy.BodyParts.ForEach(bp =>
            {
                bp.StartPhysics();
            });
        }

    }
}
