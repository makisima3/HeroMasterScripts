using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using Assets.Code.UI;
using Assets.Code.Events;

namespace Assets.Code.Entities
{
    class HandsChanger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static HandsChanger Instance { get; private set; }

        [SerializeField] private RectTransform content;
        [SerializeField] private float timeToMove = 0.1f;

        private float leftHorizontalBorder;
        private float rightHorizontalBorder;

        private List<HandsIcon> childs;
        private new RectTransform transform;
        private float centerX;

        public OnHandChange OnHandChange { get; private set; }

        private void Awake()
        {
            Instance = this;

            OnHandChange = new OnHandChange();

            transform = GetComponent<RectTransform>();

            childs = new List<HandsIcon>();

            foreach (RectTransform child in content)
            {
                childs.Add(child.GetComponent<HandsIcon>());
            }
        }

        private void Start()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //fixUnityShit

            centerX = transform.position.x + transform.sizeDelta.x / 2f;
            leftHorizontalBorder = -content.rect.width;
            rightHorizontalBorder = 0f;
        }
        public void OnDrag(PointerEventData eventData)
        {
            var position = content.anchoredPosition + Vector2.right * eventData.delta.x;

            position.x = Mathf.Clamp(position.x, leftHorizontalBorder, rightHorizontalBorder);

            content.anchoredPosition = position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var child = GetNearest();

            content.DOAnchorPosX(-child.transform.localPosition.x, timeToMove)
                .SetEase(Ease.Linear)
                .OnComplete(() => OnHandChange.Invoke(child.HandsType));           
        }

        private HandsIcon GetNearest()
        {
            float childMinDeltaX = float.MaxValue;

            HandsIcon nearestChild = null;

            foreach (var child in childs)
            {
                float currentChidleDelta = Mathf.Abs(centerX - child.transform.position.x);

                if (currentChidleDelta < childMinDeltaX)
                {
                    nearestChild = child;
                    childMinDeltaX = currentChidleDelta;
                }
            }

            return nearestChild;
        }

    }
}
