//Copyright 2020 OpenRace
//License Type: MIT
//File Authors: Arkadiusz Tołwiński

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenRace.MapEditor {
    public class ObjectSelection : MonoBehaviour {

        public List<GameObject> props = new List<GameObject>();
        Dictionary<string, GameObject> propsByName = new Dictionary<string, GameObject>();
        Dictionary<string, Button> buttonsByName = new Dictionary<string, Button>();

        public GameObject button;
        public Transform buttonParent;

        string selectedObject = "";

        public void Select(string objectName) {

            if(buttonsByName.ContainsKey(selectedObject)) {
                buttonsByName[selectedObject].interactable = true;
            }

            selectedObject = objectName;

            if(buttonsByName.ContainsKey(selectedObject)) {
                buttonsByName[selectedObject].interactable = false;
            }
        }

        public GameObject GetSelectedObject() {
            if(propsByName.ContainsKey(selectedObject))
                return propsByName[selectedObject];
            return null;
        }

        void Awake() {
            ToStatic.objectSelection = this;   
        }

        void Start() {

            float y = button.GetComponent<RectTransform>().rect.height;

            for(int i = 0; i < props.Count; i++) {
                GameObject newButton = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity);
                newButton.transform.SetParent(buttonParent);
                newButton.transform.localPosition = new Vector2(0, -y * i);

                string name = props[i].name;

                Button newButtonButton = newButton.GetComponent<Button>();
                buttonsByName.Add(name, newButtonButton);

                newButton.GetComponent<Button>().onClick.AddListener(delegate { Select(name); });

                newButton.GetComponentInChildren<Text>().text = Name(name);

                propsByName.Add(name, props[i]);
            }
        }

        string Name(string currentName) {

            string result = "";

            if(currentName.Length >= 1)
                result += currentName[0];

            for(int i = 1; i < currentName.Length; i++) {

                char c = currentName[i];

                if(char.IsUpper(c) && currentName[i - 1] != ' ')
                    result += " ";
                result += c;
            }

            return result;
        }
    }
}
