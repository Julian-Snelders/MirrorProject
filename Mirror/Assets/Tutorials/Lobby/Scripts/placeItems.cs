using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class placeItems : NetworkBehaviour
{
    [SerializeField] private GameObject ItemListUI;                     // UI Object for item list
    private bool ItemListOn = false;                                    // bool that keeps state of item list

    public GameObject ghost_Cube;                                       // prefab that follows raycast hit for placement
    public GameObject placed_Cube;                                      // prefab that will be spawned onclick on Ghost_Cube position
    public Camera cam;                                                  // camera variable for raycast origin reference
    GameObject ghost;

    bool pressedcube = false;                                           // cube button has been pressed
    RaycastHit hit;                                 

    
    public void PressCube()                                             // method called on cube button click
    {
       
        pressedcube = true;                                             // pressed cube button
       
        Instantiate(ghost_Cube, hit.point, Quaternion.identity);        // instantiate ghost cube at hit position
        ghost = GameObject.FindWithTag("ghost");                        // private object that makes reference to ghost cube object
        ItemMenuOff();
    }
    
    public void Update()
    {
        ItemMenuOn();

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) // forward line is drawen in infinite direction
        {       
            if (pressedcube == true)                                    // Cube bottun was pressed
            {
                ghost.transform.position = hit.point;                   // ghost_cube follows hit.point

                if (Input.GetMouseButtonDown(0))                        // left mouse spawns placed_cube 
                {
                    CmdSpawn();
                }       
                if (Input.GetMouseButtonDown(1))                        // right mouse destroys ghost_cube prefab
                {
                    Destroy(ghost);
                    pressedcube = false;
                }
                return;
            }
        }
        
    }
    void CmdSpawn()
    {
       GameObject PlaceDaCube = Instantiate(placed_Cube, ghost.transform.position, ghost.transform.rotation);  // instantiate placed object on ghost cube position
       NetworkServer.Spawn(PlaceDaCube);                                               // spawn for networkmanager
    }
    
    void ItemMenuOn()                                                                   // active like update
    {
        if (hasAuthority)                                                               // if player has authority
        {
            if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == false) // turn on UI.      //press Q to turn on ItemList UI Object
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
