using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject controller;
    public float followSpeed;
    public float zoomMagnitude;
    public float zoomSpeed;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        targetZoom = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    float targetZoom;
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, controller.transform.position, followSpeed * Time.deltaTime);
        if(Input.mouseScrollDelta.y != 0) {
            targetZoom += Input.mouseScrollDelta.y * zoomMagnitude * -1;
            targetZoom = Mathf.Clamp(targetZoom, 5f, 100f);

        }
        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
    
}
