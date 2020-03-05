//Copyright 2020 OpenRace
//License Type: MIT
//File Authors: Arkadiusz Tołwiński

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenRace.MapEditor {
    public class Move : MonoBehaviour {

        public float speed = 1;
        public float mouseSpeed = 2;

        float modifiedSpeed = 1;

        float x;
        float y;

        void Update() {

            if(Input.GetButton("Fire2")) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                modifiedSpeed += Input.GetAxis("Mouse ScrollWheel");

                x += Input.GetAxis("Mouse X") * mouseSpeed;
                y -= Input.GetAxis("Mouse Y") * mouseSpeed;

                transform.eulerAngles = new Vector3(y, x, 0);
            } else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        void FixedUpdate() {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed * modifiedSpeed);
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed * modifiedSpeed);
        }
    }
}
