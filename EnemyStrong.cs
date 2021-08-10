using Assets.Code.AbilitySettings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.Entities.Enemies
{
    public class EnemyStrong : Enemy
    {
        [SerializeField] private List<Rigidbody> bones;

        [Space]
        [SerializeField] private float hp = 2f;
        [SerializeField] private float wakeUpDelay = 2f;
        [SerializeField] private int webCountForStop = 6;


        [Space]
        [SerializeField] private int headLayerNumber = 6;
        [SerializeField] private float attackDelay = 0.5f;
        [SerializeField] private SkinnedMeshRenderer webTrap1;
        [SerializeField] private SkinnedMeshRenderer webTrap2;

        private new Rigidbody rigidbody;
        private Animator animator;
        private List<BodyPart> bodyParts;
        private NavMeshAgent agent;
        private bool isAttackPlayer = false;
        private bool isDead = false;
        private bool isStay = true;

        private Coroutine wakeUpCorutine;

        private float speedStep;

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
                b.ParentArrowHandler = this;
                b.ParentMagicWaveHandler = this;
                b.ParentEnemy = this;
            });

            speedStep = agent.speed / webCountForStop;
        }

        private void Update()
        {
            if (isAttackPlayer)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Player.Instance.Die();
                }
            }
        }

        public override void Attack(Vector3 playerPosition)
        {
            agent.SetDestination(playerPosition);
            animator.SetTrigger("walk");

            StartCoroutine(AwaitNavmesh());
        }

        //ожидание установки Destanation
        private IEnumerator AwaitNavmesh()
        {
            yield return new WaitForSeconds(attackDelay);


            isAttackPlayer = true;
        }

        public override void Hadle(WebAbilitySettings settings)
        {
            Debug.Log("enemyStrenghtHandlWeb");

            agent.speed -= speedStep;

            var trap1Weight = webTrap1.GetBlendShapeWeight(0);
            var trap2Weight = webTrap2.GetBlendShapeWeight(0);

            if (trap1Weight < 100)
            {
                webTrap1.gameObject.SetActive(true);
                webTrap1.SetBlendShapeWeight(0, trap1Weight + 35f);
            }
            else if(trap2Weight < 100)
            {
                webTrap1.SetBlendShapeWeight(0, 100f);

                webTrap2.gameObject.SetActive(true);
                webTrap2.SetBlendShapeWeight(0, webTrap1.GetBlendShapeWeight(0) + 35f);
            }
            else
            {
                webTrap2.SetBlendShapeWeight(0, 100f);
                animator.SetTrigger("idle");
            }

            if (agent.speed <= 0)
                agent.speed = 0;
        }

        public override void Hadle(ArrowAbilitySettings settings)
        {
            Debug.Log("enemyStrenghtHandlArrow");

            animator.enabled = false;          

            if (settings.HitLayer == headLayerNumber)
                Hit(hp);
            else
            {
                Hit(settings.DamageToEnemyStrong);
            }
        }

        public override void Hadle(MagicWaveAbilitySettings settings)
        {
            Debug.Log("enemyStrongHandlWave");
            animator.enabled = false;            

            Hit(settings.DamageToEnemyStrong);
        }

        public override void Hadle(LaserAbilitySettings settings)
        {
            Debug.Log("enemyStrongHandlLaser");
            animator.enabled = false;            

            Hit(settings.DamageToEnemyStrong);
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

            MyWaypoint.enemyDeath.Invoke(this);
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
            agent.isStopped = true;
        }
    }
}