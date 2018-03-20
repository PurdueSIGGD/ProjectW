using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameHudController : MonoBehaviour {
	public RectTransform healthBar;
	public RectTransform magicBar;
	public Animator hitMarker;
    public Image[] teamColoredImages;
	public GameObject abilityItemPrefab;
	public RectTransform abilityHolder;
    public Text healthText;
    public Text magicText;

}
