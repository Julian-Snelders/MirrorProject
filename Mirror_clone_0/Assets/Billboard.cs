using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Billboard : MonoBehaviour
{
    public Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
    }

    private void LateUpdate()
   {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180f, 0f);
       // transform.Rotate(0f, cameraToLookAt.transform.position.y, 0f);

            //  transform.LookAt(cameraToLookAt.transform.rotation * Vector3.forward, cameraToLookAt.transform.rotation * Vector3.up);

     //   Vector3 v = cameraToLookAt.transform.position - transform.position;
     //   v.x = v.z = 0.0f;
     //   transform.LookAt(cameraToLookAt.transform.position - v);
   }
}
