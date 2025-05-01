using System;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {
    Camera cam;
    
    [Serializable]
    public class TagCallback {
        public string tag;
        public Action<GameObject> callback;
    }

    // Exposed in code (not inspector)
    private readonly Dictionary<string, Action<GameObject>> tagCallbacks = new();

    [Tooltip("Max raycast distance")]
    public float rayDistance = 100f;

    [Tooltip("LayerMask for raycasting")]
    public LayerMask raycastLayers = Physics.DefaultRaycastLayers;
    private void Awake() {
        cam = Camera.main;
    }
    // Register a callback for a tag
    public void RegisterTagCallback(string tag, Action<GameObject> callback) {
        tagCallbacks[tag] = callback;
    }

    // Unregister a tag
    public void UnregisterTagCallback(string tag) {
        if (tagCallbacks.ContainsKey(tag))
            tagCallbacks.Remove(tag);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, raycastLayers)) {
                string hitTag = hit.collider.tag;
                if (tagCallbacks.TryGetValue(hitTag, out var callback)) {
                    callback?.Invoke(hit.collider.gameObject);
                }
            }
        }
    }
}
