using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : PlayerComponent {
    /*
     * For all abilities a player can have, we add them like a component to the player
     * Whenever the user wants to use them, we call the method "use"
     * In that use, you can do whatever you want. Call upon other components through base player, etc.
     */
    public abstract void use(); 

}
