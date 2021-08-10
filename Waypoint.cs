using Assets.Code.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Entities
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private Transform playerPlace;
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private Transform pointToLookAt;

        public Transform PlayerPlace => playerPlace;
        public Transform PointToLookAt => pointToLookAt;

        public EnemyDeath enemyDeath { get; private set; }
        public bool IsLastWaypoint { get; set; }
        public List<Enemy> Enemies => enemies;

        private void Awake()
        {
            enemyDeath = new EnemyDeath();

            enemyDeath.AddListener(RemoveEnemy);

            enemies.ForEach(e => e.MyWaypoint = this);
        }

        public void AttackPlayer()
        {
            enemies.ForEach(e => e.Attack(Player.Instance.transform.position));
        }

        public void Check()
        {
            if (enemies.Count <= 0)
                Player.Instance.goToNextWaypoint.Invoke();
        }

        private void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
            Check();
        }
    }
}