using UnityEngine;

public class ExampleClickHandler : MonoBehaviour {
    public RaycastManager raycastManager;
    private GameObject card;
    private GameObject tile;
    void Start() {
        raycastManager.RegisterTagCallback("Card", OnCardClicked);
        raycastManager.RegisterTagCallback("Tile", OnTileClicked);
    }

    void OnCardClicked(GameObject obj) {
        Debug.Log("Enemy clicked: " + obj.name);
        // Custom logic like obj.GetComponent<Health>().TakeDamage();
        card = obj;
    }

    void OnTileClicked(GameObject obj) {
        Debug.Log("Item clicked: " + obj.name);
        // Maybe: Inventory.Add(obj);
        if(card == null) {
            Debug.Log("Card must be selected");
            return;
        }
        card.transform.SetParent(obj.transform, false);
        card.transform.localPosition = new Vector3(0, 0, 0);
        card = null;
        tile = null;
    }
}