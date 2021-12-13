using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class placeItems : NetworkBehaviour
{
    [SerializeField] private GameObject ItemListUI;
    private bool ItemListOn = false;

    public GameObject ghost_Cube;
    public GameObject placed_Cube;
    public Camera cam;
    GameObject ghost;

    bool pressedcube = false;    // cube button has been pressed
    RaycastHit hit;

    public override void OnStartAuthority()
    {
        cam.ScreenPointToRay(Input.mousePosition);
    }
    public void PressCube()     // method called on cube button click
    {
        pressedcube = true;
       
        Instantiate(ghost_Cube, hit.point, Quaternion.identity);
        ghost = GameObject.FindWithTag("ghost");
        ItemMenuOff();
    }
    [Client]
    public void Update()
    {
        ItemMenuOn();

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) // forward line is drawen in infinite direction
        {       
            if (pressedcube == true)
            {
                ghost.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(placed_Cube, ghost.transform.position, ghost.transform.rotation);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(ghost);
                    pressedcube = false;
                }
                return;
            }
        }
        
    }
    void ItemMenuOn()
    {
        if (hasAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == false) // turn on UI.
            {
                gameObject.GetComponent<PlayerMovementController>().enabled = false;          

                ItemListUI.SetActive(true);

                ItemListOn = true;
                Cursor.lockState = CursorLockMode.None;

            }

            else if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == true) // turn off UI.
            {
                ItemMenuOff();
            }
        }
    }
    void ItemMenuOff()
    {
        gameObject.GetComponent<PlayerMovementController>().enabled = true;
        ItemListUI.SetActive(false);

        ItemListOn = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
