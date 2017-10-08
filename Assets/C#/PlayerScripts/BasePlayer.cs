using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Rigidbody))]
public class BasePlayer : MonoBehaviour {
    [HideInInspector]
    public PlayerInput myInput;
    [HideInInspector]
    public PlayerMovement myMovement;
    [HideInInspector]
    public PlayerStats myStats;
    [HideInInspector]
    public Animator myAnimator;
    [HideInInspector]
    public Rigidbody myRigid;
    [HideInInspector]
    public PlayerAbility[] myAbilities;
	// Use this for initialization
	void Start () {
        myInput = (PlayerInput)GetComponent<PlayerInput>().initialize(this);
        myMovement = (PlayerMovement)GetComponent<PlayerMovement>().initialize(this);
        myStats = (PlayerStats)GetComponent<PlayerStats>().initialize(this);
        myAnimator = this.GetComponentInChildren<Animator>();
        myRigid = GetComponent<Rigidbody>();
        myAbilities = GetComponents<PlayerAbility>();
        foreach (PlayerAbility playerAbility in myAbilities) {
            playerAbility.initialize(this);
        }
	}
	
}
