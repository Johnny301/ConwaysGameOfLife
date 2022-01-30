using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite leftClick;
    public Sprite rightClick;
    public Sprite middleClick;
    public Sprite normal;

    public Image cursorImage;

    public Vector2 cursorHotspot;
    void Start()
    {
        //Cursor.visible = false;
        //cursorHotspot = new Vector2(normal.width / 8, normal.height / 8);
        cursorImage.rectTransform.sizeDelta = new Vector2(10f, 10f);
        cursorImage.transform.position= new Vector3(0f, 0f, cursorImage.transform.position.z);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //cursorImage.sprite = leftClick;
            //Cursor.SetCursor(leftClick, cursorHotspot, CursorMode.ForceSoftware);

        } else if (Input.GetMouseButtonDown(1)) {
            //Cursor.SetCursor(rightClick, cursorHotspot, CursorMode.ForceSoftware);
        } else if (Input.GetMouseButtonDown(2)) {
          //  Cursor.SetCursor(middleClick, cursorHotspot, CursorMode.ForceSoftware);
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) {
        //    Cursor.SetCursor(normal, cursorHotspot, CursorMode.ForceSoftware);
        }
        
        
        float camZoom = Camera.main.orthographicSize;
        
        Vector3 camPos = Camera.main.transform.position;
        Vector3 newPos = new Vector3(
            Mathf.Clamp(cursorImage.transform.position.x + Input.GetAxis("Mouse X")*1.3f, -Screen.width/2, Screen.width/2),
            Mathf.Clamp(cursorImage.transform.position.y + Input.GetAxis("Mouse Y")*1.3f, -Screen.height/2, Screen.height/2),
            cursorImage.transform.position.z);
        Debug.Log($"{cursorImage.transform.position} - {Input.GetAxis("Mouse X")*1.3f}, {newPos} - {Screen.width/2} : {Screen.height/2}");
        cursorImage.gameObject.transform.position = Vector3.Lerp(cursorImage.transform.position,
            newPos,
            Time.deltaTime * 50f);

        /*cursorImage.gameObject.transform.position = Vector3.Lerp( cursorImage.gameObject.transform.position,
            new Vector3((mousePos.x + camPos.x) / camZoom,
            (mousePos.y + camPos.y) / camZoom,
            cursorImage.gameObject.transform.position.z), Time.deltaTime * 20f);*/
        
        
    }
}
