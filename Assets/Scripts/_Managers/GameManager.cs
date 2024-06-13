using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private enum State{
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public static GameManager Instance { get; private set;}
    private State currentState;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimerMax = 60f;
    private float gamePlayingTimer;
    private bool isGamePaused = false;
    private void Awake() {
        Instance = this;
        currentState = State.WaitingToStart;
    }

    private void Start() {
        GameInput.Instance.OnPausedAction += GameInput_OnPausedAction;
    }

    private void GameInput_OnPausedAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    private void Update() {
        switch(currentState) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer < 0){
                    ChangeState(State.CountdownToStart);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if(countdownToStartTimer < 0){
                    gamePlayingTimer = gamePlayingTimerMax;
                    ChangeState(State.GamePlaying);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer < 0){
                    ChangeState(State.GameOver);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void ChangeState(State newState){
        currentState = newState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsCountdownToStartActive(){
        return currentState == State.CountdownToStart;
    }

    public bool IsGamePlaying(){
        return currentState == State.GamePlaying;
    }

    public bool IsGameOver(){
        return currentState == State.GameOver;
    }

    public float GetCountdownToStartTimer(){
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized(){
        return 1 -(gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame(){
        isGamePaused = !isGamePaused;
        if(isGamePaused){
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
