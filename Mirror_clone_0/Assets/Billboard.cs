using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Billboard : MonoBehaviour // script on TMP text
{
    public Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
    }

    private void LateUpdate() // late Update happens after the normal update. everything will be updated and afterwards the text component turns to our camera.
   {
        transform.LookAt(cameraToLookAt.transform); // text component aims at camera
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180f, 0f); // freeze all movement except the Y movement and rotate it 180 degrees
   }
}
