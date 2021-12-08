using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeItems : MonoBehaviour
{
    public GameObject ItemListUI;
    public bool ItemListOn = false;
    void Update()
    {
        Debug.Log("bool is" + ItemListOn);

        if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == false)
        {
            Debug.Log("Set to true");
            ItemListUI.SetActive(true);

            ItemListOn = true;
        }

        else if (Input.GetKeyDown(KeyCode.Q) && ItemListOn == true)
        {
            Debug.Log("Set to false");
            ItemListUI.SetActive(false);

            ItemListOn = false;
        }

    }
}
