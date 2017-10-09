using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Effect_SpeedBoost : NetworkBehaviour {
    public PlayerMovement target;
    public float speedIncreasePercentage;
    private float endTime;
    GameObject startSound, endSound;

	public void Initialize(PlayerMovement target, float percentage, float duration, GameObject startSound, GameObject endSound) {
        this.target = target;
        speedIncreasePercentage = percentage;
        endTime = Time.time + duration;
        target.sRunSpeedModifier += speedIncreasePercentage;
        this.startSound = startSound;
        this.endSound = endSound;
        CmdSpawnBoost(this.startSound);
        //EditorApplication.isPaused = true;
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
        result.transform.localPosition = Vector3.zero; // Center on parent
        //NetworkServer.Spawn(result);
    }
}
