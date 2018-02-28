using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItem : MonoBehaviour
{
    public int id;
    public Image classIcon;
    public Text nameText;
    public Text score;
    public Text deaths;
    public Text assists;
    public Text ping;
    public Image[] teamImages;
    public GameObject referredItem;
}
