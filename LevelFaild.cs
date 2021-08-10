using Assets.Code.Entities;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    public class LevelFaild : MonoBehaviour
    {

        [InspectorName("Elements")]
        [SerializeField] private Image back;
        [SerializeField] private Image levelFaildImage;
        [SerializeField] private Button noThanksButton;
        [SerializeField] private Button tryAgainButton;

        [Space]
        [InspectorName("Elements")]
        [SerializeField] private float blackOutForce = 0.6f;
        [SerializeField] private float blackOutTime = 1f;
        [SerializeField] private float noThanksShowingDelay = 1f;


        public void Show()
        {
            //TimeManager.Instanse.StopTime();

            back.DOFade(blackOutForce, blackOutTime)
                .OnComplete(() =>
                {
                    levelFaildImage.gameObject.SetActive(true);
                    tryAgainButton.gameObject.SetActive(true);
                    StartCoroutine(Delay());
                });
        }

        public void TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
            //TimeManager.Instanse.StartTime();
        }

        public void NoThanks()
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCount - 1)
            {
                SceneManager.LoadScene(0);
                //TimeManager.Instanse.StartTime();
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //TimeManager.Instanse.StartTime();
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(noThanksShowingDelay);

            noThanksButton.gameObject.SetActive(true);
        }
    }
}