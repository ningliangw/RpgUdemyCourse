using System;
using UnityEngine;
using UnityEngine.UI;

public class SmallHpBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;
    private FlipSprite flipSprite;
    public Damageable damageable;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        flipSprite = GetComponentInParent<FlipSprite>();
        slider = GetComponentInChildren<Slider>();
        if (damageable == null)
        {
            damageable = GetComponentInParent<Damageable>();
        }
    }

    private void OnEnable()
    {
        if (flipSprite != null)
        {
            flipSprite.OnFlip += Flip;
        }
        damageable.OnTakeDamage += UpdateHpBar;
        damageable.OnHeal += UpdateHpBar;
    }

    private void OnDisable()
    {
        if (flipSprite != null)
        {
            flipSprite.OnFlip -= Flip;
        }
        damageable.OnHeal -= UpdateHpBar;
    }

    private void UpdateHpBar(GameObject object1, GameObject object2)
    {
        slider.maxValue = damageable.GetMaxHealthValue();
        slider.value = damageable.currentHp;
    }

    private void UpdateHpBar()
    {
        slider.maxValue = damageable.GetMaxHealthValue();
        slider.value = damageable.currentHp;
    }

    private void Flip()
    {
        rectTransform.Rotate(0, 180, 0);
    }
}