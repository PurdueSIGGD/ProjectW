using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUI : PlayerComponent {
    public bool isPaused;

    public float pauseCooldown = 1; // Cooldown, in seconds
    private float lastUse = -100; // Last time we used it, in seconds;
    private bool shouldBeLocked;

    public void Start() {
        UnPauseGame();
    }
    void LateUpdate() {
        // Unity is buggy as fuck sometimes
        // You have to click back on the window in order to be locked again
        if (shouldBeLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void TogglePause() {
        if (Time.unscaledTime - lastUse > pauseCooldown) {
            Pause();
            lastUse = Time.unscaledTime;
        }
    }
    private void Pause() {
        isPaused = !isPaused;
        if (isPaused) {
            PauseGame();
        } else {
            UnPauseGame();
        }
    }
    private void PauseGame() {
        shouldBeLocked = false;

    }
    private void UnPauseGame() {
        shouldBeLocked = true;

    }
}
