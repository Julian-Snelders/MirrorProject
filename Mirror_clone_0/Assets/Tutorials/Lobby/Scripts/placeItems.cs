using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class placeItems : NetworkBehaviour
{
    [SerializeField] private GameObject[] GhostShapes;                          // array for Ghost objects
    [SerializeField] private GameObject[] placeableShapes;                      // array for Placed objects

    [SyncVar] int GhostIndex;                                                     // index number for specific ghost shape
    [SyncVar] int PlacedIndex;                                                    // index number for specific placed shape

    [SerializeField] private GameObject ItemListUI;                     // UI Object for item list
    private bool ItemListOn = false;                                    // bool that keeps state of item list
    bool during = false;                                                // bool that keeps u from opening itemlist when ghost object is active

    public Camera cam;                                                  // camera variable for raycast origin reference
    GameObject ghost;                                                   // reference object for spawning and following raycast hit

    bool pressedShape = false;                                          // if the UI button has been pressed
    RaycastHit hit;

    public override void OnStartAuthority()
    {
        enabled = true;                                                 // enable on correct player
    }

  
    public void PressCircle()                                           // method called on sphere button click
    {
        if (!hasAuthority) { return; }
        GhostIndex = 0;
        PressedShape();
    }

    public void PressCube()                                             // method called on Cube button click
    {
        if (!hasAuthority) { return; }
        GhostIndex = 1;
        PressedShape();
    }
 
    public void PressCylinder()                                        // method called on Cilinder button click
    {
        if (!hasAuthority) { return; }
        GhostIndex = 2;
        PressedShape();
    }
  
    public void PressStair()                                           // method called on Stairs button click
    {
        if (!hasAuthority) { return; }
        GhostIndex = 3;
        PressedShape();
    }

    void PressedShape()                                                // pressed a button method
    {
        pressedShape = true;

        ItemListOn = false;
        ItemListUI.SetActive(false);                                   // UI object false

        during = true;

        Instantiate(GhostShapes[GhostIndex], hit.point, Quaternion.identity); // instantiate ghost object of correct index.

        ghost = GameObject.FindWithTag("ghost");                        // private object that makes reference to current ghost object


        ItemMenuOff();
    }

  
    void Update()
    {
        if (!hasAuthority) { return; }

            PlacedIndex = GhostIndex;                                        // there the same all the time

        ItemMenuOn();

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) // forward line is drawen in infinite direction
        {

            if (pressedShape == true)                                    // if bottun was pressed
            {
                ghost.transform.position = hit.point;                    // ghost_cube follows hit.point
                Debug.DrawRay(cam.transform.position, ghost.transform.TransformDirection(Vector3.forward), Color.green);

                if (Input.GetMouseButtonDown(0))                         // left mouse spawns placed_object
                {
                    CmdSpawn(GhostIndex, ghost.transform.position, Quaternion.identity);
                }                      // Spawn 
                if (Input.GetMouseButtonDown(1))                        // right mouse destroys ghost_object prefab
                {
                    Destroy(ghost);
                    pressedShape = false;

                    ItemListUI.SetActive(true);
                    ItemListOn = true;
                    during = false;

                    gameObject.GetComponent<PlayerMovementController>().enabled = false; // disable movement
                    Cursor.lockState = CursorLockMode.None;                              // cursor onscreen
                }                      // Destroy ghost object
                return;
            }
        }

    }

    [Command]
    void CmdSpawn(int index, Vector3 position, Quaternion rotation)
    {
           GameObject PlaceDaShape = Instantiate(placeableShapes[index], position, rotation);  // instantiate placed object on 0, 0, 0
           NetworkServer.Spawn(PlaceDaShape, connectionToClient);

           CmdChangePosition(PlaceDaShape);
    }

    [Command]
    void CmdChangePosition(GameObject Shape)                                                   // get instantiated item and place at ghost position
    {
        if (connectionToClient != null)
        {
            Shape.transform.position = ghost.transform.position;
        }
    }


    void ItemMenuOn()                                                                   // active like update
    {
        if (hasAuthority)                                                               // if player has authority
        {
            if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == false && during == false)  //press Q to turn on ItemList UI Object
            {
                gameObject.GetComponent<PlayerMovementController>().enabled = false;    // turn off movement

                ItemListUI.SetActive(true);
                ItemListOn = true;

                Cursor.lockState = CursorLockMode.None;                                 // cursor onscreen

            }

            else if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == true)                 // press Q to turn off ItemList UI Object
            {
                ItemMenuOff();
            }
        }
    }
    void ItemMenuOff()
    {
        gameObject.GetComponent<PlayerMovementController>().enabled = true;             // enable movement
        ItemListUI.SetActive(false);
        ItemListOn = false;

        Cursor.lockState = CursorLockMode.Locked;                                       // cursor offscreen
    }
}
