using Assets.Code.AbilitySettings;
using Assets.Code.Entities.AbilityImpls;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.HandsImpls
{
    public class HawkEyeHands : Hands
    {
        [SerializeField] private ArrowAbility HawkEyeAbilityPrefab;
        [SerializeField] private ArrowAbilitySettings abilitySettings;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Animator animator;
        [Space]
        [SerializeField] private float shootDelay = 1f;

        private bool isReload = false;
        public override void Shoot(Vector3 target)
        {
            if (isReload)
                return;

            isReload = true;

            animator.SetTrigger("hawkeye");
            transform.LookAt(target);

            var HawkEyeAbility = Instantiate(HawkEyeAbilityPrefab.gameObject, spawnPoint.position, spawnPoint.rotation, spawnPoint.parent).GetComponent<ArrowAbility>();

            StartCoroutine(delay(target, HawkEyeAbility));
        }      

        public override void StopShoot(Vector3 target)
        {

        }

        private IEnumerator delay(Vector3 target, ArrowAbility arrowAbility)
        {
            yield return new WaitForSeconds(shootDelay);

            arrowAbility.transform.SetParent(null);         

            abilitySettings.Target = target;

            arrowAbility.Activate(abilitySettings);

            isReload = false;
        }
    }
}