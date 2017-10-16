using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : PlayerComponent {
    public bool isPaused;

    public float pauseCooldown = 1; // Cooldown, in seconds
    private float lastUse = -100; // Last time we used it, in seconds;
    private bool shouldBeLocked;

    public GameObject rootGUI;
    public RectTransform healthBar;
    public RectTransform magicBar;

    public override void PlayerComponent_Start() {
        UnPauseGame();
        if (isLocalPlayer) {
            // Don't want enemy GUIs on top of ours
            rootGUI.SetActive(true);
        } else {
            rootGUI.SetActive(false);
        }
    }
    public override void PlayerComponent_Update() {
        if (isLocalPlayer) {
            // Set health and magic in GUI
            healthBar.localScale = new Vector2(myBase.myStats.health / myBase.myStats.healthMax, 1);
            magicBar.localScale = new Vector2(myBase.myStats.magic / myBase.myStats.magicMax, 1);

            // Update current abilities and their cooldowns
        }
    }

    void LateUpdate() {
        if (isLocalPlayer) {
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
        

    }
    public void Death() {
        rootGUI.SetActive(false);
    }

    public void TogglePause() {
        if (isLocalPlayer) {
            if (Time.unscaledTime - lastUse > pauseCooldown) {
                Pause();
                lastUse = Time.unscaledTime;
            }
        }
    }
    private void Pause() {
        if (isLocalPlayer) {
            isPaused = !isPaused;
            if (isPaused) {
                PauseGame();
            } else {
                UnPauseGame();
            }
        }
    
        
    }
    private void PauseGame() {
        shouldBeLocked = false;

    }
    private void UnPauseGame() {
        shouldBeLocked = true;

    }
}
