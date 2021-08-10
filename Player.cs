using Assets.Code.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Assets.Code.UI;

namespace Assets.Code.Entities
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }


        [SerializeField] NavMeshAgent agent;
        [SerializeField] List<Waypoint> waypoints;
        [SerializeField] private float delayBetweenWaypoints = 2f;

        [Space]
        [SerializeField] private float lookAtSpeed = 0.2f;

        public GoToNextWaypoint goToNextWaypoint { get; private set; }
        public GetWaypoint getWaypoint { get; private set; }
        public bool IsMoving => isMoving;

        private bool isMoving = false;
        private Waypoint currentWaypoint;
        private bool IsDead = false;

        private void Awake()
        {
            Instance = this;

            goToNextWaypoint = new GoToNextWaypoint();
            getWaypoint = new GetWaypoint();

            goToNextWaypoint.AddListener(GoToPoint);
            getWaypoint.AddListener(OnWaypoint);

            waypoints.Last().IsLastWaypoint = true;
        }

        private void Start()
        {
            goToNextWaypoint.Invoke();
        }

        private void Update()
        {
            if (isMoving)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    getWaypoint.Invoke();

                    agent.isStopped = true;

                    isMoving = false;
                }
            }
        }


        private void GoToPoint()
        {
            if (waypoints.Count <= 0)
            {
                Debug.Log("Victory!");

                UIManager.Instance.LevelComleteShow();

                return;
            }

            currentWaypoint = waypoints.FirstOrDefault();

            StartCoroutine(DeleayBetwenWaypoints());

            waypoints.Remove(currentWaypoint);

        }

        private void OnWaypoint()
        {
            transform.DOLookAt(currentWaypoint.PointToLookAt.position, lookAtSpeed, AxisConstraint.Y).OnComplete(currentWaypoint.AttackPlayer);
        }

        private IEnumerator DeleayBetwenWaypoints()
        {
            yield return new WaitForSeconds(delayBetweenWaypoints);

            agent.SetDestination(currentWaypoint.PlayerPlace.position);
            agent.isStopped = false;
            isMoving = true;
        }

        public void Die()
        {
            if (IsDead)
                return;

            Debug.Log("PlayerDie");

            UIManager.Instance.LevelFaildShow();

            agent.isStopped = true;
            currentWaypoint.Enemies.ForEach(e => e.Stop());

            IsDead = true;
        }
    }
}