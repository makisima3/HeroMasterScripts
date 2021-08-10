using Assets.Code.Entities;
using Assets.Code.Enums;
using Assets.Code.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputCatcher : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static InputCatcher Instance { get; private set; }

    [SerializeField] private LayerMask layerMask;

    public OnTap OnTap { get; private set; }

    public OnBeginDrag OnBeginDragEvent { get; private set; }
    public OnDrag OnDragEvent { get; private set; }
    public OnEndDrag OnEndDragEvent { get; private set; }

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;

        mainCamera = Camera.main;

        OnTap = new OnTap();

        OnBeginDragEvent = new OnBeginDrag();
        OnDragEvent = new OnDrag();
        OnEndDragEvent = new OnEndDrag();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;
        OnTap.Invoke(RaycastFrom(eventData.position));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnBeginDragEvent.Invoke(RaycastFrom(eventData.position));
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent.Invoke(RaycastFrom(eventData.position));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndDragEvent.Invoke(RaycastFrom(eventData.position));
    }

    /// <summary>
    /// Cast a ray from screen point and return world point
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <returns></returns>
    private Vector3 RaycastFrom(Vector2 screenPoint, float maxDistance=1000f)
    {
        var ray = mainCamera.ScreenPointToRay(screenPoint);
        Vector3 worldPoint;
        if (Physics.Raycast(ray, out RaycastHit info, maxDistance, layerMask))
            worldPoint = info.point;
        else
            worldPoint = ray.GetPoint(maxDistance);

        return worldPoint;
    }

}
