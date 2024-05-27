using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {
    [SerializeField] private Image timerImage;
    [SerializeField] private Gradient colorGradient;
    private void Update() {
        float timer = 1 - GameManager.Instance.GetGamePlayingTimerNormalized();
        timerImage.color = colorGradient.Evaluate(timer);
        timerImage.fillAmount = timer;
    }
}
