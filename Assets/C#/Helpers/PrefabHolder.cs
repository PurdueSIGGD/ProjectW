using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour {
    /**
     * A simple class used to store prefabs inside of unity for easy generation
     * In case you have multiple classes that rely on a list of prefabs, but they won't be saved across each class
     */
    public GameObject[] prefabs;
}
