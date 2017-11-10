using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToTrack : MonoBehaviour {
	public enum ItemTrackType { Generic, Player, Objective };
    public Sprite sprite;
	public ItemTrackType trackType;
}
