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
    private PlayerAbility[] myAbilities;
    void Start() {
        // We gather abilities here. If you want to add a new ability at runtime, you must run RepopulateAbilities()
        RepopulateAbilities();
        ability_Start();
    }
    public void RepopulateAbilities() {
        myAbilities = this.GetComponents<PlayerAbility>();
    }
    [SyncVar]
    int used;
    int hasUsed;
    /* it's a pain in the ass because there's a bug where two components of the same type can't get a command referenced to a specific one */
    /* so we pass the string of the type... because you can't pass abstract parameters */
    [Command]
    public void CmdUse(String p) {
        foreach (PlayerAbility myP in myAbilities) {
            if (myP.GetType().ToString() == p) {
                myP.useAbility();
            }
        }
    }
    public void useAbility() {
        used++;
    }
    void Update() {
        if (used != hasUsed) {
            //print("server has told us to use");
            hasUsed++;
            use();
        }
        ability_Update();
    }
    void Death() {
        used = 0;
        hasUsed = 0;
    }
    /* these are the other methods you must implement. Can be empty, there for your own benefit */
    public abstract void ability_Start(); // Called when the object is alive
    public abstract void ability_Update(); // Called once every frame
    public abstract void use();  // Called when either your input or the server tells you to use these

}
