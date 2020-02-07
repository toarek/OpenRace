using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenRace.Car {

    public class Test : MonoBehaviour {

        [Header("Wheels")]
        public List<WheelCollider> torqueWheels = new List<WheelCollider>();
        public List<WheelCollider> steeringWheels = new List<WheelCollider>();

        List<Transform> torqueWheelsTransform = new List<Transform>();
        List<Transform> steeringWheelsTransform = new List<Transform>();

        [Header("Options")]
        public float maxMotorTorque;
        public float maxSteeringAngle;

        public Text text;

        void Awake() {

            for(int i = 0; i < torqueWheels.Count; i++) {
                torqueWheelsTransform.Add(torqueWheels[i].transform);
            }
            for(int i = 0; i < steeringWheels.Count; i++) {
                steeringWheelsTransform.Add(steeringWheels[i].transform);
            }

        }
        
        public void FixedUpdate() {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

            for(int i = 0; i < torqueWheels.Count; i++) {
                torqueWheels[i].motorTorque = motor;
                torqueWheelsTransform[i].Rotate(new Vector3(0, torqueWheels[i].rpm / 60 * 360 * Time.deltaTime, 0));
            }

            for(int i = 0; i < steeringWheels.Count; i++) {
                steeringWheels[i].steerAngle = steering;
                steeringWheelsTransform[i].localEulerAngles = new Vector3(90, 90 + steering, 0);
                steeringWheelsTransform[i].Rotate(new Vector3(0, steeringWheels[i].rpm / 60 * 360 * Time.deltaTime, 0));
            }

            if(Input.GetKey(KeyCode.Space)) {
                for(int i = 0; i < torqueWheels.Count; i++) {
                    torqueWheels[i].brakeTorque = 1000000;
                }
            } else {
                for(int i = 0; i < torqueWheels.Count; i++) {
                    torqueWheels[i].brakeTorque = 0;
                }
            }

            float circumFerence = 2.0f * 3.14f * 0.25f;
            float speedOnKmh = (circumFerence * torqueWheels[0].rpm) * 60;

            text.text = speedOnKmh.ToString();

        }
    }
}
