using Assets.Code.Enums;
using Assets.Code.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Entities
{
    public class HandsManager : MonoBehaviour
    {
        [System.Serializable]
        public class HandsTypeObjectPair
        {
            public Hands Hands;

            public HandsType Type;
        }

        [SerializeField] private List<HandsTypeObjectPair> hands;
        [SerializeField] private HandsType startHandsType;

        private Hands currentHands;
        private HandsType currentHandsType;
        private Dictionary<HandsType, Hands> handsTypeToHands;

        public OnHandsTypeChange OnHandsTypeChange { get; private set; }

        private void Awake()
        {
            OnHandsTypeChange = new OnHandsTypeChange();

            InputCatcher.Instance.OnTap.AddListener(SingleShot);

            InputCatcher.Instance.OnBeginDragEvent.AddListener(DragShot);
            InputCatcher.Instance.OnDragEvent.AddListener(DragShot);
            InputCatcher.Instance.OnEndDragEvent.AddListener(StopShot);

            HandsChanger.Instance.OnHandChange.AddListener(ChangeHands);

            handsTypeToHands = hands.ToDictionary(pair => pair.Type, pair => pair.Hands);            

            ChangeHands(startHandsType);
        }

        public void ChangeHands(HandsType type)
        {
            if (currentHands != null)
                currentHands.Hide();

            currentHandsType = type;
            currentHands = handsTypeToHands[currentHandsType];

            currentHands.Show();

            OnHandsTypeChange.Invoke(currentHandsType);
        }

        private void SingleShot(Vector3 target)
        {
            if (Player.Instance.IsMoving)
                return;

            if (currentHandsType != HandsType.IronMan)
            {
                currentHands.Shoot(target);
            }
        }

        private void DragShot(Vector3 target)
        {
            if (Player.Instance.IsMoving)
                return;

            if (currentHandsType == HandsType.IronMan)
            {
                currentHands.Shoot(target);
            }
        }

        private void StopShot(Vector3 target)
        {            
            if (currentHandsType == HandsType.IronMan)
            {
                currentHands.StopShoot(target);
            }
        }
    }
}