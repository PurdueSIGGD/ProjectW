using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerAbility : PlayerComponent {
    /*
     * For all abilities a player can have, we add them like a component to the player
     * Whenever the user wants to use them, we call the method "use"
     * In that use, you can do whatever you want. Call upon other components through base player, etc.
     * 
     * In order to preserve network behavior, used and hasUsed track the amount of times that it has been called
     * If a client notices used has changed, they execute the method and try again.
     */
    public float magicDraw; // Has to be handled in your player ability class. Can be total amt, can be magica per second, etc.
	public Sprite abilitySprite;
	[HideInInspector]
	public AbilityIcon abilityIcon;

    private static string USE_METHOD_NAME = "Use";
    
    public override void PlayerComponent_Start() {
        ResgisterDelegate(USE_METHOD_NAME, UseWrapper);
        ability_Start();
    }
    
   
    public override void PlayerComponent_Update() {
        
        ability_Update();
    }
    /**
     * Tell all instances of this player to use this ability
     */
    public void ClientsUse() {
        Buf buf = new Buf();
        buf.methodName = USE_METHOD_NAME;
        NotifyAllClientDelegates(buf);
    }
    void Death() {

    }
    public void UseWrapper(Buf data) {
        use();
    }
    /* these are the other methods you must implement. Can be empty, there for your own benefit */
    public abstract void ability_Start(); // Called when the object is alive
    public abstract void ability_Update(); // Called once every frame
    public abstract void use();  // Called when either your input or the server tells you to use these

	public void SetIcon(AbilityIcon icon) {
		abilityIcon = icon;
	}	
}
