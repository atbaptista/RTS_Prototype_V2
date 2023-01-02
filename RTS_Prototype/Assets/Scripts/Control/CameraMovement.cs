using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform startingLoc;

    public float scrollSpeed = 10f;
    public float zoomSpeed = 50f;

    public float maxCamHeight = 54f;
    public float minCamHeight = -4f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = new Vector3(startingLoc.position.x,
                startingLoc.transform.position.y, startingLoc.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        //center camera on player with spacebar
        if (Input.GetKey(KeyCode.Space))
        {
            /*transform.position = new Vector3(player.position.x, 
                transform.position.y, player.position.z - 5);*/
        }

        //mouse at top of screen
        if(mousePos.y >= 0.95 * Screen.height)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
        }
        //mouse at bottom of screen
        if (mousePos.y <= 0.05 * Screen.height)
        {
            transform.Translate(Vector3.back * Time.deltaTime * scrollSpeed, Space.World);
        }
        //mouse at right of screen
        if (mousePos.x >= 0.95 * Screen.width)
        {
            transform.Translate(Vector3.right * Time.deltaTime * scrollSpeed);
        }
        //mouse at left of screen
        if (mousePos.x <= 0.05 * Screen.width)
        {
            transform.Translate(Vector3.left * Time.deltaTime * scrollSpeed);
        }

        //scroll up
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * zoomSpeed);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,
            minCamHeight, maxCamHeight), transform.position.z);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.Translate(Vector3.down * Time.deltaTime * zoomSpeed);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,
            minCamHeight, maxCamHeight), transform.position.z);
        }
    }
}
