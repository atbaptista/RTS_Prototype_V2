using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{

#region Fields

    [Header("Selection")]
    private LinkedList<GameObject> prevSelected = new LinkedList<GameObject>();
    [SerializeField] private RectTransform boxImage;

    private Vector3 selectStart;
    private Vector3 selectEnd;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool _aPressed;
    private bool _shiftPressed;

    [SerializeField] private Camera cam;

    [Header("Cursor")]
    public Texture2D SelectCursor;
    public Texture2D AttackCursor;
    private Vector2 _cursorOffset;

    [Header("Command FX")]
    public GameObject ComFX;
    public Color AMoveColor;
    public Color MoveColor;

    [HideInInspector] public bool isPaused = false;

#endregion Fields

    private void Start()
    {
        // gui stuff
        boxImage.gameObject.SetActive(false);
        Invoke("SelectEverything", 0.5f);

        _aPressed = false;
        _shiftPressed = false;

        // cursor 
        _cursorOffset = new Vector2(SelectCursor.width / 3, SelectCursor.height / 6);
        Cursor.SetCursor(SelectCursor, _cursorOffset, CursorMode.Auto);
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        Checks();
        Lmb();
        RmbMove();
    }

#region Update Methods

    private void Checks()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _aPressed = true;
            Cursor.SetCursor(AttackCursor, _cursorOffset, CursorMode.Auto);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectEverything();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _shiftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _shiftPressed = false;
        }

        //cursor
        if (!_aPressed)
        {
            Cursor.SetCursor(SelectCursor, _cursorOffset, CursorMode.Auto);
        }
    }

#region LMB

    private void Lmb()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LmbDown();
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //gui
            boxImage.gameObject.SetActive(false);
            return;
        }

        //lmb held down
        if (Input.GetMouseButton(0))
        {
            LmbHeldDown();
        }
    }

    private void LmbDown()
    {
        int ignoreUIMask = 1 << 5;
        ignoreUIMask = ~ignoreUIMask;

        // mouse start pos
        startPos = Input.mousePosition;
        RaycastHit hit = RaycastMousePosition(ignoreUIMask);
        selectStart = hit.point;

        // attack move
        if (_aPressed)
        {
            // create the command fx
            CreateCommandFX(hit.point, AMoveColor);

            bool isRobot = false;
            foreach (GameObject i in prevSelected)
            {
                if (i.Equals(null))
                {
                    continue;
                }
                isRobot = i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Robot);
                if (isRobot)
                {
                    // if ever add different kinds of units move all the amove checks to moveable component
                    i.GetComponent<George>().AMove(hit);
                }
            }
        }
        // if lmb on robot while not a-moving
        else if (hit.collider.CompareTag("MoveObject") && hit.collider.GetComponent<Selectable>().unitType == Selectable.unitTypes.Robot)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // append to selected
                if (!hit.collider.gameObject.GetComponent<Selectable>().isSelected)
                {
                    hit.collider.gameObject.GetComponent<Selectable>().isSelected = true;
                    prevSelected.AddLast(hit.collider.gameObject);

                }
                // remove from selected
                else
                {
                    hit.collider.gameObject.GetComponent<Selectable>().isSelected = false;
                    prevSelected.Remove(hit.collider.gameObject);
                }
                return;
            }

            ClearPrevSelected();

            // if not already selected, select it
            if (!hit.collider.gameObject.GetComponent<Selectable>().isSelected)
            {
                hit.collider.gameObject.GetComponent<Selectable>().isSelected = true;
                prevSelected.AddLast(hit.collider.gameObject);
            }
        }
        else
        {
            // lmb on floor
            ClearPrevSelected();
        }
        _aPressed = false;
    }

    private void LmbHeldDown()
    {
        int ignoreUIMask = 1 << 5;
        ignoreUIMask = ~ignoreUIMask;
        Collider[] newlySelected;

        CreateUIRectangle();

        // selector code
        RaycastHit hit = RaycastMousePosition(ignoreUIMask);
        selectEnd = hit.point;

        // Selectable layermask (7)
        int layerMask = 1 << 7;

        // midpoint formula, calculate the middle of the selection box
        Vector3 centerOfOverlap;
        centerOfOverlap.x = (selectStart.x + selectEnd.x) / 2;
        centerOfOverlap.y = 0.25f;
        centerOfOverlap.z = (selectStart.z + selectEnd.z) / 2;

        // half extents 
        Vector3 halfExtents = (selectStart - selectEnd) / 2;
        halfExtents.x = Mathf.Abs(halfExtents.x);
        halfExtents.z = Mathf.Abs(halfExtents.z);

        // spawn overlapbox to detect everything inside of the mouse click/drag
        newlySelected = Physics.OverlapBox(centerOfOverlap, halfExtents,
            Quaternion.identity, layerMask);

        // add new selected to prev selected
        foreach (Collider i in newlySelected)
        {
            // if not already selected, select it
            if (!i.gameObject.GetComponent<Selectable>().isSelected)
            {
                i.gameObject.GetComponent<Selectable>().isSelected = true;
                prevSelected.AddLast(i.gameObject);
            }
        }
    }

    #endregion LMB

    private void RmbMove()
    {
        if (!Input.GetMouseButtonDown(1))
        {
            return;
        }
        _aPressed = false;

        bool isRobot = false;

        // Bit shift the index of the ground layer (8) to get a bit mask
        int layerMask = 1 << 8;
        RaycastHit hit = RaycastMousePosition(layerMask);

        // create the command fx
        CreateCommandFX(hit.point, MoveColor);

        foreach (GameObject i in prevSelected)
        {
            if (i.Equals(null))
            {
                continue;
            }

            // should check if it's Moveable if stationary units are ever added
            isRobot = i.GetComponent<Selectable>().unitType.Equals(Selectable.unitTypes.Robot);
            if (isRobot)
            {
                if (_shiftPressed)
                {
                    // add location to the move queue 
                    i.GetComponent<Moveable>().QueueMovement(hit.point);
                }
                else
                {
                    // clear move queue and move to the location
                    i.GetComponent<Moveable>().ClearMoveQueue();
                    i.GetComponent<Moveable>().GoTo(hit.point);
                }
            }
        }
    }

#endregion Update Methods

    private void CreateUIRectangle()
    {
        // object not already active, make it active
        if (!boxImage.gameObject.activeInHierarchy)
        {
            boxImage.gameObject.SetActive(true);
        }

        endPos = Input.mousePosition;

        Vector3 boxStart = Camera.main.WorldToScreenPoint(startPos);
        boxStart.z = 0f;

        Vector3 center = (startPos + endPos) / 2f;

        boxImage.position = center;

        float sizeX = Mathf.Abs(startPos.x - endPos.x);
        float sizeY = Mathf.Abs(startPos.y - endPos.y);

        boxImage.sizeDelta = new Vector2(sizeX, sizeY);
    }

    // Spawns a glowing ring at point with color 
    private void CreateCommandFX(Vector3 point, Color FXColor)
    {
        point.y = 0;
        GameObject tempFX = Instantiate(ComFX, point, Quaternion.identity);
        tempFX.GetComponent<CommandFX>().CircleColor = FXColor;
    }

    // Raycasts from the mouse position on the screen to a point in the game space
    private RaycastHit RaycastMousePosition(int layerMask)
    {
        RaycastHit hit;

        // point in game where player clicks
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            // make sure game always has background
            return hit;
        }
        throw new InvalidOperationException("raycast bad");
    }

    private void SelectEverything()
    {
        foreach (Selectable i in FindObjectsOfType<Selectable>())
        {
            if (!i.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (i.GetComponent<Selectable>().unitType == Selectable.unitTypes.Robot)
            {
                // if not already selected, select it
                if (!i.GetComponent<Selectable>().isSelected)
                {
                    i.GetComponent<Selectable>().isSelected = true;
                    prevSelected.AddLast(i.gameObject);
                }
            }
        }
    }

    private void ClearPrevSelected()
    {
        // clear previously selected objects
        for (LinkedListNode<GameObject> node = prevSelected.First; node != null; node = node.Next)
        {
            // null check if enemies die while selected
            if (!node.Value.Equals(null))
            {
                node.Value.GetComponent<Selectable>().isSelected = false;
            }
        }
        prevSelected.Clear();
    } 

}
