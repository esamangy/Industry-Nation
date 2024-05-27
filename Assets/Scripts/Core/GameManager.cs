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
    public static GameManager Instance { get; private set;}
    private State currentState;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 10f;
    private void Awake() {
        Instance = this;
        currentState = State.WaitingToStart;
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
        print(currentState);
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

    public float GetCountdownToStartTimer(){
        return countdownToStartTimer;
    }
}
