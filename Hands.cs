using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Hands : MonoBehaviour
    {
        public abstract void Shoot(Vector3 target);
        public abstract void StopShoot(Vector3 target);

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    }
}