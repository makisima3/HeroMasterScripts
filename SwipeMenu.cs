using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Code.UI
{
    public class SwipeMenu : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {

        [SerializeField]private Scrollbar scrollbar;

        private float scroll_pos = 0;
        private float[] pos;
        private bool isEndDrag;
        private float distance;

        public void OnBeginDrag(PointerEventData eventData)
        {
           
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Drag");
            scroll_pos = scrollbar.value;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("EndDrag");
            isEndDrag = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            pos = new float[transform.childCount];
            distance = 1f / (pos.Length - 1f);
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = distance * i;
            }
        }

        // Update is called once per frame
        void Update()
        {


            if (isEndDrag)
            {
                

                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], 0.1f);
                    }
                }



                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        Debug.LogWarning("Current Selected Level" + i);
                        transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                        for (int j = 0; j < pos.Length; j++)
                        {
                            if (j != i)
                            {
                                transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                            }
                        }
                    }
                }

                isEndDrag = false;
            }
        }
    }
}