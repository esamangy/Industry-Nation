using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour {
    [SerializeField] private LoadingDock loadingDock;
    private const string POPUP = "Popup";
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image[] animationStuff;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start(){
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailed += DeliveryManager_OnOrderFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnOrderFailed(object sender, DeliveryManager.DeliveryEventArgs e) {
        if(e.loadingDock != loadingDock){
            //different dock
            return;
        }
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = failedColor;
        SetAnimationStuffColor(failedColor);
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnOrderSuccess(object sender,  DeliveryManager.DeliveryEventArgs e) {
        if(e.loadingDock != loadingDock){
            //different dock
            return;
        }
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = successColor;
        SetAnimationStuffColor(successColor);
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";
    }

    private void SetAnimationStuffColor(Color color){
        foreach(Image image in animationStuff){
            image.color = color;
        }
    }
}
