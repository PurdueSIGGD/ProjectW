using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_SpeedBoost : MonoBehaviour {
    public PlayerMovement target;
    public float speedIncreasePercentage;
    private float endTime;


	public void Initialize(PlayerMovement target, float percentage, float duration) {
        this.target = target;
        speedIncreasePercentage = percentage;
        endTime = Time.time + duration;
        target.runSpeedModifier += speedIncreasePercentage;
    }

	// Update is called once per frame
	void Update () {
		if (Time.time >= endTime) {
            target.runSpeedModifier -= speedIncreasePercentage;
            Destroy(this);
        }
	}
}
