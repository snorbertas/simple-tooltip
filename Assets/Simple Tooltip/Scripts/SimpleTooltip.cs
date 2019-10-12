using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class SimpleTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SimpleTooltipStyle simpleTooltipStyle;
    [TextArea] public string infoLeft = "Hello";
    [TextArea] public string infoRight = "";
    private STController tooltipController;
    private EventSystem eventSystem;
    private bool cursorInside = false;
    private bool isUIObject = false;
    private bool showing = false;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        tooltipController = FindObjectOfType<STController>();
        if (!tooltipController)
        {
            tooltipController = AddTooltipPrefabToScene();
        }
        if (!tooltipController)
        {
            Debug.LogWarning("Could not load the tooltip.");
        }

        if (GetComponent<RectTransform>())
            isUIObject = true;

        // Always make sure there's a style loaded
        if (!simpleTooltipStyle)
            simpleTooltipStyle = Resources.Load<SimpleTooltipStyle>("STDefault");
    }

    private void Update()
    {
        if (!cursorInside)
            return;

        tooltipController.ShowTooltip();
    }

    private static STController AddTooltipPrefabToScene()
    {
        return Instantiate(Resources.Load<GameObject>("Tooltip")).GetComponentInChildren<STController>();
    }

    private void OnMouseOver()
    {
        if (isUIObject)
            return;

        if (eventSystem)
        {
            if (eventSystem.IsPointerOverGameObject())
            {
                HideTooltip();
                return;
            }
        }
        ShowTooltip();
    }

    private void OnMouseExit()
    {
        if (isUIObject)
            return;
        HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isUIObject)
            return;
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isUIObject)
            return;
        HideTooltip();
    }

    public void ShowTooltip()
    {
        showing = true;
        cursorInside = true;
        tooltipController.SetCustomStyledText(infoLeft, simpleTooltipStyle, STController.TextAlign.Left);
        tooltipController.SetCustomStyledText(infoRight, simpleTooltipStyle, STController.TextAlign.Right);
        tooltipController.ShowTooltip();
    }

    public void HideTooltip()
    {
        if (!showing)
            return;
        showing = false;
        cursorInside = false;
        tooltipController.HideTooltip();
    }

    private void Reset()
    {
        if (!simpleTooltipStyle)
            simpleTooltipStyle = Resources.Load<SimpleTooltipStyle>("STDefault");
        if (GetComponent<RectTransform>())
            return;

        if (GetComponent<Collider>())
            return;

        gameObject.AddComponent<BoxCollider>();
    }
}
