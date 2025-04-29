using UnityEngine;

public class ClickableObject : MonoBehaviour {
    void OnMouseDown() {
        Debug.Log("Clicked on " + gameObject.name);
    }
}