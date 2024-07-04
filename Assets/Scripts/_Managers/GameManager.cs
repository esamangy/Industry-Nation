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
    public event EventHandler<PauseStatusEventArgs> OnGamePausedStatus;
    public static GameManager Instance { get; private set;}
    private State currentState;
    private float gameStartDelay = 0f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimerMax = 60f;
    private float gamePlayingTimer;
    private PlayerController pausingPlayer;
    private List<PlayerController> players;
    public class PauseStatusEventArgs : EventArgs {
        public bool isPaused;
    }
    private void Awake() {
        Instance = this;
        currentState = State.WaitingToStart;
        pausingPlayer = null;
    }

    private void Start() {
        gamePlayingTimer = gamePlayingTimerMax;
        players = PlayersManager.Instance.GetPlayers();
        PlayersManager.Instance.OnPlayerListChanged += PlayersManager_OnPlayerListChanged;

        StartCoroutine(StartGame(gameStartDelay));
    }

    private void PlayersManager_OnPlayerListChanged(object sender, EventArgs e) {
        foreach (PlayerController player in players) {
            if(player != null){
                player.OnPlayerPaused -= PlayerController_OnPlayerPaused;
            }
        }

        players = PlayersManager.Instance.GetPlayers();
        foreach(PlayerController player in players){
            player.OnPlayerPaused += PlayerController_OnPlayerPaused;
        }
        UpdatePauseStatus();
    }

    private void PlayerController_OnPlayerPaused(object sender, EventArgs e) {
        if(pausingPlayer == null){
            // no one is pausing
            pausingPlayer = sender as PlayerController;
        } else {
            //someone is pauseing
            if(pausingPlayer == sender as PlayerController){
                // the person who is pause has unpaused
                pausingPlayer = null;
            }
        }

        foreach(PlayerController player in players){
            if(player == pausingPlayer){
                PlayersManager.Instance.SetControllingPlayer(player);
            } else {
                PlayersManager.Instance.RemoveControlFromPlayer(player);
            }
        }
        UpdatePauseStatus();
    }

    private IEnumerator StartGame(float delay){
        yield return new WaitForSeconds(delay);
        ChangeState(State.CountdownToStart);
    }

    private void Update() {
        switch(currentState) {
            case State.WaitingToStart:
                
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
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public bool IsPaused(){
        return pausingPlayer != null;
    }
    public void UpdatePauseStatus(){
        if(pausingPlayer == null){
            // not paused
            Time.timeScale = 1;
            OnGamePausedStatus?.Invoke(this, new PauseStatusEventArgs{
                isPaused = false,
            });
        } else {
            // is paused
            Time.timeScale = 0;
            OnGamePausedStatus?.Invoke(this, new PauseStatusEventArgs{
                isPaused = true,
            });
        }
    }

    public void ResumeGame(){
        pausingPlayer = null;
        UpdatePauseStatus();
    }
}
