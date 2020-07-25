using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEntity : MonoBehaviour {
    public void Kill() {
        Debug.Log("Killed");
    }

    public void GivePoints(float points) {
        Debug.Log("Points: " + points.ToString());
    }
}
