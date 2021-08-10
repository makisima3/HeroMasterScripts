using Assets.Code.AbilitySettings;
using Assets.Code.Entities.AbilityImpls;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.HandsImpls
{
    public class DRStrageHands : Hands
    {
        [SerializeField] private MagicWaveAbility magicWaveAbilityPrefab;
        [SerializeField] private MagicWaveAbilitySettings abilitySettings;
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

            animator.SetTrigger("drstrange");
            //transform.LookAt(target);

            var magicWaveAbility = Instantiate(magicWaveAbilityPrefab.gameObject, spawnPoint.position, Quaternion.identity, null).GetComponent<MagicWaveAbility>();

            StartCoroutine(delay(target, magicWaveAbility));
        }

        public override void StopShoot(Vector3 target)
        {
            
        }

        private IEnumerator delay(Vector3 target, MagicWaveAbility magicWaveAbility)
        {
            yield return new WaitForSeconds(shootDelay);

                  

            abilitySettings.Target = target;

            magicWaveAbility.Activate(abilitySettings);

            isReload = false;
        }
    }
}