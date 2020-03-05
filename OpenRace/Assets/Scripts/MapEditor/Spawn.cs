//Copyright 2020 OpenRace
//License Type: MIT
//File Authors: Arkadiusz Tołwiński

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenRace.MapEditor {
    public class Spawn : MonoBehaviour {

        void Update() {

            if(!PointOverUI.IsPointerOverUIElement() && Input.GetButtonDown("Fire1")) {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out RaycastHit hit)) {

                    GameObject element = ToStatic.objectSelection.GetSelectedObject();

                    

                    if(element != null) {
                        GameObject newElement = Instantiate(element, new Vector3(hit.point.x, hit.point.y, hit.point.z) + Vector3.up, Quaternion.identity);
                        newElement.transform.eulerAngles = element.transform.eulerAngles;
                    }
                }

            }
        }
    }
}
