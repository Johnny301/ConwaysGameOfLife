using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float mouseSpeed;
    public float speedMod;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    Vector3 camDragStartPos;
    void Update()
    {
        if (Camera.main == null)
            return;
        float currentMaxSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            currentMaxSpeed *= speedMod;
        }
        if (Input.GetMouseButtonDown(2)) {
            camDragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2)) {

            Vector3 difference = camDragStartPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            transform.position = Vector3.Lerp(transform.position, transform.position + (difference*mouseSpeed), Time.deltaTime * 100f);
            camDragStartPos = Vector3.Lerp(camDragStartPos, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 100f);
        }
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) {
            Vector3 input = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * currentMaxSpeed), 
                transform.position.y + (Input.GetAxis("Vertical") * currentMaxSpeed), 
                0f);
            transform.position = Vector3.Lerp(transform.position, input, Time.deltaTime * 100f);
        }
    }
}
