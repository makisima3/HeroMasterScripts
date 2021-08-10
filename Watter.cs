using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities
{
    public class Watter : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<BodyPart>(out var enemy))
            {
                Debug.Log("water kill someone");
                enemy.ParentEnemy.Hit(enemy.ParentEnemy.HP);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<BodyPart>(out var enemy))
            {
                Debug.Log("water kill someone");
                enemy.ParentEnemy.Hit(enemy.ParentEnemy.HP);
            }
        }
    }
}