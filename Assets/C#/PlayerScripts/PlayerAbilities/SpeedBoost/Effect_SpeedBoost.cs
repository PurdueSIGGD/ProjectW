using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Effect_SpeedBoost : NetworkBehaviour {
    public PlayerMovement target;
    public float speedIncreasePercentage;
    private float endTime;
    private AudioClip start, end;
    private GameObject endSound;

	public void Initialize(PlayerMovement target, float percentage, float duration, GameObject startSound, GameObject endSound) {
        this.target = target;
        speedIncreasePercentage = percentage;
        endTime = Time.time + duration;
        target.sRunSpeedModifier += speedIncreasePercentage;
        CmdSpawnBoost(startSound);
        this.endSound = endSound;
    }

	// Update is called once per frame
	void Update () {
		if (Time.time >= endTime) {
            target.sRunSpeedModifier -= speedIncreasePercentage;
            CmdSpawnBoost(endSound);
            Destroy(this);
        }
	}
    //[Command]
    public void CmdSpawnBoost(GameObject toInstantiate) {
        GameObject result = GameObject.Instantiate(toInstantiate, target.transform);
        //NetworkServer.Spawn(result);
    }
}
