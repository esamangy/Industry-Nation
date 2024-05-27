using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingTextUI : MonoBehaviour {
    [SerializeField] private string baseLoadingText;
    private float timer;
    private void Start() {
        StartCoroutine(LoadingTextEffect());
    }

    private IEnumerator LoadingTextEffect(){
        while(true){
            timer += Time.deltaTime;
            string periods = "";
            for(int i = 0; i < timer % 3; i ++){
                periods += ".";
            }
            GetComponent<TextMeshProUGUI>().text = baseLoadingText + periods;
            yield return null;
        }
    }
}
