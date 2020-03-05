﻿//Copyright 2020 OpenRace
//License Type: MIT
//File Authors: Arkadiusz Tołwiński

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OpenRace.MapEditor {
    public static class PointOverUI {
        public static bool IsPointerOverUIElement() {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults) {
            for(int index = 0; index < eventSystemRaysastResults.Count; index++) {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if(curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }

        static List<RaycastResult> GetEventSystemRaycastResults() {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
