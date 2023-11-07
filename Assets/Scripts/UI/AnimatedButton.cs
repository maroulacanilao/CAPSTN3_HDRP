using System;
using System.Collections;
using CustomHelpers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler, IDeselectHandler
{
    public Button button;
    public float animationDuration = 0.3f;
    public Ease animationEase = Ease.OutQuad;
    public string selectSfxName = "ButtonSelect";
    public string clickSfxName = "ButtonClick";

    public Image selectIcon;

    private Vector3 originalScale;

    [SerializeField] private bool WantToAnimate = true;

    private void Awake()
    {
        if(button == null) button = GetComponent<Button>();
        originalScale = button.transform.localScale;
        if(selectIcon != null) selectIcon.gameObject.SetActive(false);
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
        if (selectIcon != null) selectIcon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(button == null) button = GetComponent<Button>();
        if(button == null) return;
        button.transform.localScale = originalScale;
        if (selectIcon != null) selectIcon.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        button.transform.localScale = originalScale;
    }

    private void OnClick()
    {
        if(this.IsEmptyOrDestroyed()) return;
        if(button.IsEmptyOrDestroyed()) return;
        
        StopAllCoroutines();
        if(!gameObject.activeInHierarchy) return;
        Managers.AudioManager.PlaySfx(clickSfxName);
        StartCoroutine(Co_AnimateButton(originalScale * 0.9f));
    }

    private void OnHover()
    {
        if(this.IsEmptyOrDestroyed()) return;
        if(button.IsEmptyOrDestroyed()) return;
        if(!button.interactable) return;
        if(button.transform.localScale != originalScale) return;

        StopAllCoroutines();
        if(!gameObject.activeInHierarchy) return;
        Managers.AudioManager.PlaySfx(selectSfxName);
        if(selectIcon != null && button.interactable) selectIcon.gameObject.SetActive(true);
        StartCoroutine(Co_AnimateButton(originalScale * 1.1f));
    }

    private void OnIdle()
    {
        if(this.IsEmptyOrDestroyed()) return;
        if(button.IsEmptyOrDestroyed()) return;
        if(button.transform.localScale == originalScale) return;
        StopAllCoroutines();
        if (selectIcon != null) selectIcon.gameObject.SetActive(false);
        StartCoroutine(Co_AnimateButton(originalScale));
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        OnIdle();
    }
    public void OnSelect(BaseEventData eventData)
    {
        OnHover();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        OnIdle();
    }
    
    private IEnumerator Co_AnimateButton(Vector3 targetScale_)
    {
        
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / animationDuration;
            float easedT = EasingFunction(t);

            button.transform.localScale = Vector3.Lerp(originalScale, targetScale_, easedT);
            yield return null;
        }

        button.transform.localScale = targetScale_;
    }

    private float EasingFunction(float t)
    {
        // Apply desired easing function here
        return t * t * (3f - 2f * t); // Quadratic easing
    }
}
