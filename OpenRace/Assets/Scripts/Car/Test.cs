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
        public Text text1;
        public Text text2;
        public Text text3;
        public Text text4;
        public Text text5;

        int maxRPM = 1900;
        float maxSpeed = 350f;

        public WheelCollider WheelL;
        public WheelCollider WheelR;
        public float AntiRoll = 0.0f;

        bool to100 = true;
        float to100Time = 0;

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

                if(torqueWheels[i].rpm > maxRPM) {
                    torqueWheels[i].motorTorque = 0;
                }

                if((100f * torqueWheels[i].rpm * 0.001885f) > maxSpeed) {
                    torqueWheels[i].brakeTorque = 100000;
                } else {
                    torqueWheels[i].brakeTorque = 0;
                }
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

            float speedOnKmh = 100f * steeringWheels[0].rpm * 0.001885f;
            float speedOnKmhTorque = 100f * torqueWheels[0].rpm * 0.001885f;
            float speed = this.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

            text.text = "RPM: " + Mathf.RoundToInt(steeringWheels[0].rpm).ToString();
            text3.text = "Torque RPM: " + Mathf.RoundToInt(torqueWheels[0].rpm).ToString();
            text1.text = "Wheel Speed: " + Mathf.RoundToInt(speedOnKmh).ToString();
            text4.text = "Torque Wheel Speed: " + Mathf.RoundToInt(speedOnKmhTorque).ToString();
            text2.text = "Real Speed: " + Mathf.RoundToInt(speed).ToString();

            if(false) {
                WheelHit hit;
                float travelL = 1.0f;
                float travelR = 1.0f;

                bool groundedL = WheelL.GetGroundHit(out hit);
                if(groundedL)
                    travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

                bool groundedR = WheelR.GetGroundHit(out hit);
                if(groundedR)
                    travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

                float antiRollForce = (travelL - travelR) * AntiRoll;

                if(groundedL)
                    this.GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                           WheelL.transform.position);

                if(groundedR)
                    this.GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * antiRollForce,
                           WheelR.transform.position);
            }

            if(to100) {
                if(speed > 1)
                    to100Time += Time.deltaTime;
                text5.text = "Time to 100 km/h: " + (Mathf.RoundToInt(to100Time*100f)/100f).ToString();
                if(speed >= 100)
                    to100 = false;
            }
        }
    }
}
