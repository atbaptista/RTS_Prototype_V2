using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Changelevel : MonoBehaviour
{
    public Texture2D crosshair;

    // Start is called before the first frame update
    void Start()
    {
        //set the cursor origin to its centre. (default is upper left corner)
        Vector2 cursorOffset = new Vector2(crosshair.width / 3, crosshair.height / 6);

        //Sets the cursor to the Crosshair sprite with given offset 
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        //Invoke("Unloaders", .1f);
    }
    private void Unloaders()
    {
        SceneManager.UnloadSceneAsync("First_Level");
    }
}
