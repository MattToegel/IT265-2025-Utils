using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            this.transform.Translate(Vector3.left * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            this.transform.Translate(Vector3.right * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W)) {
            this.transform.Translate(Vector3.forward * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S)) {
            this.transform.Translate(Vector3.back * Time.deltaTime);
        }
    }
}
