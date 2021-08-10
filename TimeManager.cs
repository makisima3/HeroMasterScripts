using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Code.Entities
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instanse { get; private set; }

        [SerializeField] private float timeMoveDuration = 1f;

        private void Awake()
        {
            Instanse = this;
        }

        public void StopTime()
        {
            DOTween.To(TimeGetter, TimeSetter, 0f, timeMoveDuration)
                .OnComplete(() => Time.timeScale = 0f);
        }

        public void StartTime()
        {
            DOTween.To(TimeGetter, TimeSetter, 1f, timeMoveDuration)
                .OnComplete(() => Time.timeScale = 0f);
        }

        private void TimeSetter(float value)
        {
            Time.timeScale = value;
        }

        private float TimeGetter()
        {
            return Time.timeScale;
        }
    }
}