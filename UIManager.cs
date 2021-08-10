using System.Collections;
using UnityEngine;

namespace Assets.Code.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private LevelComlete levelComleteViwe;
        [SerializeField] private LevelFaild levelFaildViwe;

        private void Awake()
        {
            Instance = this;
        }

        public void LevelComleteShow()
        {
            levelComleteViwe.gameObject.SetActive(true);
            levelComleteViwe.Show();
        }

        public void LevelFaildShow()
        {
            levelFaildViwe.gameObject.SetActive(true);
            levelFaildViwe.Show();
        }
    }
}