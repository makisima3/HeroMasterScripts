using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Ability<TSettings>: MonoBehaviour
    {
        public abstract void Activate(TSettings settings);

        public abstract void Deactivate(TSettings settings);

    }
}