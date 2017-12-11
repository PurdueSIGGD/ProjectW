using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour {
    // Base class for gamemode types
    public abstract ProjectWGameManager.Winner checkWinCondition();
}
