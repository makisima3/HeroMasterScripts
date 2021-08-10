using Assets.Code.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Events
{
    public class EnemyDeath : UnityEvent<Enemy>
    {
    }
}