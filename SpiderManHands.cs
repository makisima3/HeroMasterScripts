using Assets.Code.AbilitySettings;
using Assets.Code.Entities.AbilityImpls;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.HandsImpls
{
    public class SpiderManHands : Hands
    {
        [SerializeField] private WebAbility spiderManAbilityPrefab;
        [SerializeField] private WebAbilitySettings abilitySettings;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Animator animator;
        [Space]
        [SerializeField] private float shootDelay = 1f;
        public override void Shoot(Vector3 target)
        {
            animator.SetTrigger("spiderman");

            StartCoroutine(delay(target));
        }

        public override void StopShoot(Vector3 target)
        {

        }

        private IEnumerator delay(Vector3 target)
        {
            yield return new WaitForSeconds(shootDelay);

            transform.LookAt(target);

            var spiderManAbility = Instantiate(spiderManAbilityPrefab.gameObject, spawnPoint.position, Quaternion.identity, null).GetComponent<WebAbility>();

            abilitySettings.Target = target;

            spiderManAbility.Activate(abilitySettings);
        }
    }
}