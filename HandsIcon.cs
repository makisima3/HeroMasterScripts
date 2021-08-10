using Assets.Code.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Code.UI
{
    public class HandsIcon : MonoBehaviour
    {
        [SerializeField] private HandsType handsType;

        public HandsType HandsType => handsType;
        public new RectTransform transform { get; private set; }

        private void Awake()
        {
            transform = GetComponent<RectTransform>();
        }
    }
}