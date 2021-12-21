using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class placeItems : NetworkBehaviour
{
    public List<GameObject> GhostShapes;                                // list of ghost objects
    public List<GameObject> placeableShapes;                            // list of placed objects

    int GhostIndex;                                                     // index number for specific ghost shape
    int PlacedIndex;                                                    // index number for specific placed shape

    [SerializeField] private GameObject ItemListUI;                     // UI Object for item list
    private bool ItemListOn = false;                                    // bool that keeps state of item list
    bool during = false;                                                // bool that keeps u from opening itemlist when ghost object is active

    public Camera cam;                                                  // camera variable for raycast origin reference
    GameObject ghost;                                                   // reference object for spawning and following raycast hit

    bool pressedShape = false;                                          // if the UI button has been pressed
    RaycastHit hit;                                                     

    public void Start()
    {
        GhostShapes = new List<GameObject>(Resources.LoadAll<GameObject>("GhostShapes"));           // found in resources file 'ghostshapes'
        placeableShapes = new List<GameObject>(Resources.LoadAll<GameObject>("SpawnableShapes"));   // found in resources file 'spawnableshapes'
    }
    public override void OnStartAuthority()
    {
        enabled = true;
        
    }

    public void PressCircle()                                           // method called on sphere button click
    {
        GhostIndex = 0;
        PressedShape();
    }
    public void PressCube()                                             // method called on cube button click
    {
        GhostIndex = 1;
        PressedShape();
    }
    public void PressCylinder()
    {
        GhostIndex = 2;
        PressedShape();
    }
    public void PressStair()
    {
        GhostIndex = 3;
        PressedShape();
    }
    
    void PressedShape()                                          // pressed a button method
    {
        pressedShape = true; 
        
        ItemListOn = false;                                             
        ItemListUI.SetActive(false);                                    // UI object false

        during = true;

        Instantiate(GhostShapes[GhostIndex], hit.point, Quaternion.identity); // instantiate ghost object of correct index.

        ghost = GameObject.FindWithTag("ghost");                        // private object that makes reference to current ghost object
              
     
        ItemMenuOff();
    }

    [Client]
    void Update()
    {
        if(!isLocalPlayer) { return; }
        PlacedIndex = GhostIndex;                                        // there the same all the time

        ItemMenuOn();
      
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) // forward line is drawen in infinite direction
            {
                if (pressedShape == true)                                    // if bottun was pressed
                {
                    ghost.transform.position = hit.point;                    // ghost_cube follows hit.point

                    if (Input.GetMouseButtonDown(0))                         // left mouse spawns placed_object
                    {
                    
                        CmdSpawn();
                    }
                    if (Input.GetMouseButtonDown(1))                        // right mouse destroys ghost_object prefab
                    {
                        Destroy(ghost);
                        pressedShape = false;

                        ItemListUI.SetActive(true);
                        ItemListOn = true;
                        during = false;

                        gameObject.GetComponent<PlayerMovementController>().enabled = false; // disable movement
                        Cursor.lockState = CursorLockMode.None;                              // cursor onscreen
                    }
                    return;
                }
            }
        
    }

    [Command]
    private void CmdSpawn()
    {
         GameObject PlaceDaShape = Instantiate(placeableShapes[PlacedIndex], ghost.transform.position, ghost.transform.rotation);  // instantiate placed object on ghost object position
         NetworkServer.Spawn(PlaceDaShape); // spawn from networkmanager (clients can see)
        
    }
    
    void ItemMenuOn()                                                                   // active like update
    {
        if (isLocalPlayer)                                                               // if player has authority
        {
            Debug.Log("itemmenu on ");
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
