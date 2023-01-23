/* Copyright (C) 2023 NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class Movement : MonoBehaviour
{
   

    public void Move(InputAction.CallbackContext value)
    {
     
        Transform playerTransform = GameObject.FindWithTag("Player").transform;
        Vector3 pos = playerTransform.position;
        Vector2 moveVal = value.ReadValue<Vector2>();
        pos.y += moveVal.y *0.01f ;
        playerTransform.position = pos;
    }


  

 

}
