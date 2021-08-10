using Assets.Code.AbilitySettings;
using Assets.Code.Entities.AbilityImpls;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.HandsImpls
{
    public class IronManHands : Hands
    {

        [SerializeField] private LaserAbility lasersHolder;
        [SerializeField] private LaserAbilitySettings abilitySettings;
        [SerializeField] private Animator animator;
        [Space]
        [SerializeField] private float shootDelay = 1f;

        //private bool isReload = false;

        private bool isActive;
        private Coroutine rayTickCoroutine;

        private void Awake()
        {
            isActive = false;
        }

        public override void Shoot(Vector3 target)
        {
            //update target position by ref
            abilitySettings.Target = target;

            transform.LookAt(abilitySettings.Target);

            if (!isActive)
            {
                isActive = true;
                rayTickCoroutine = StartCoroutine(RayTick(abilitySettings.RayTickDelay));

                //set ref to settings
                lasersHolder.Activate(abilitySettings);

                animator.SetTrigger("ironman");
            }
        }

        public override void StopShoot(Vector3 target)
        {
            isActive = false;

            if (rayTickCoroutine != null)
            {
                StopCoroutine(rayTickCoroutine);
                rayTickCoroutine = null;
            }

            lasersHolder.Deactivate(abilitySettings);

            animator.SetTrigger("idle");
        }

        private IEnumerator RayTick(float delay)
        {
            yield return new WaitForSeconds(shootDelay);

            while (isActive)
            {
                transform.LookAt(abilitySettings.Target);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}