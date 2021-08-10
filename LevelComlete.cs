using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Code.Entities;

namespace Assets.Code.UI
{
    public class LevelComlete : MonoBehaviour
    {
        [InspectorName("Elements")]
        [SerializeField] private Image back;
        [SerializeField] private Image levelCompleteImage;
        [SerializeField] private Button nextLevelButton;

        [Space]
        [InspectorName("Elements")]
        [SerializeField] private float blackOutForce = 0.6f;
        [SerializeField] private float blackOutTime = 1f;
        public void Show()
        {
            //TimeManager.Instanse.StopTime();

            back.DOFade(blackOutForce, blackOutTime)
                .OnComplete(() => 
                { 
                    levelCompleteImage.gameObject.SetActive(true);
                    nextLevelButton.gameObject.SetActive(true);
                });
        }

        public void NextLevel()
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

      
    }
}