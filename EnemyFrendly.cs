using Assets.Code.AbilitySettings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.Entities.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyFrendly : Enemy
    {
        //[SerializeField] private List<Rigidbody> 

        [SerializeField] private List<Rigidbody> bones;

        [Space]
        [SerializeField] private float hp = 2f;
        [SerializeField] private float wakeUpDelay = 2f;


        [Space]
        [SerializeField] private int headLayerNumber = 6;


        private new Rigidbody rigidbody;
        private Animator animator;
        private List<BodyPart> bodyParts;
        private NavMeshAgent agent;
        private bool isAttackPlayer = false;
        private bool isDead = false;
        private bool isStay = true;

        private Coroutine wakeUpCorutine;

        public override float HP => hp;
        public override Rigidbody Rigidbody => rigidbody;
        public override List<BodyPart> BodyParts => bodyParts;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            bodyParts = GetComponentsInChildren<BodyPart>().ToList();

            bodyParts.ForEach(b =>
            {
                b.ParentWebHandler = this;
                b.ParentLaserHandler = this;
                b.ParentMagicWaveHandler = this;
                b.ParentArrowHandler = this;
                b.ParentEnemy = this;
            });

        }

        private void Update()
        {

        }

        public override void Hadle(WebAbilitySettings settings)
        {
            Debug.Log("enemyHandlWeb");
            animator.enabled = false;

            
        }

        public override void Hadle(ArrowAbilitySettings settings)
        {
            Debug.Log("enemyHandlArrow");
            animator.enabled = false;

            

            if (settings.HitLayer == headLayerNumber)
                Hit(hp);
            else
            {
                Hit(settings.DamageToEnemyNormal);
            }
        }

        public override void Hadle(MagicWaveAbilitySettings settings)
        {
            Debug.Log("enemyHandlWave");
            animator.enabled = false;
            

            Hit(settings.DamageToEnemyNormal);
        }

        public override void Hadle(LaserAbilitySettings settings)
        {
            Debug.Log("enemyHandlLaser");
            animator.enabled = false;

            

            Hit(settings.DamageToEnemyNormal);
        }

        public override void Attack(Vector3 playerPosition)
        {

        }

        public override void Hit(float damage)
        {
            isStay = false;
            agent.isStopped = true;

            hp -= damage;

            if (hp <= 0)
            {
                Die();
            }
            else
            {

                if (!isStay)
                {
                    if (wakeUpCorutine != null)
                        StopCoroutine(wakeUpCorutine);
                }

                wakeUpCorutine = StartCoroutine(WakeUpDelay());
            }
        }

        private void Die()
        {
            if (isDead)
                return;

            isDead = true;

            agent.isStopped = true;

            //MyWaypoint.enemyDeath.Invoke(this);

            Player.Instance.Die();
        }

        private IEnumerator WakeUpDelay()
        {
            yield return new WaitForSeconds(wakeUpDelay);

            if (!isDead)
            {
                transform.position = bodyParts[0].transform.position;

                animator.enabled = true;
                isStay = true;
                agent.isStopped = false;

                bodyParts.ForEach(b => b.IsShoted = false);
            }
        }

        public override void Stop()
        {
            
        }
    }
}