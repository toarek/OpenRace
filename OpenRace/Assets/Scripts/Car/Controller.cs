//Copyright 2020 OpenRace
//License Type: MIT
//File Authors: Arkadiusz Tołwiński

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenRace.Car {

    [System.Serializable]
    public struct WheelAxle {
        public Transform right;
        public Transform left;

        public bool steering;
        public bool torque;
        public bool brake;
    }

    public class Controller : MonoBehaviour {
        
        [Header("Wheels")]
        public List<WheelAxle> wheelAxies = new List<WheelAxle>();

        [Header("Properties")]
        public Rigidbody carBody;
        public float maxSpeed = 100f;
        public float clutchFriction = 0.1f;
        public float brakeFriction = 1.6f;

        [Header("Aerodynamics")]
        public float coefficientOfAirResistance = 0.4f;
        public float frontalSurfaceOfTheCar = 2.5f;

        [Header("Others")]
        public Text text;
        public Text text1;

        bool to100 = true;
        float to100Time = 0;

        float steeringAngle = 30f;

        Dictionary<Transform, MeshCollider> wheelPhysicMaterial = new Dictionary<Transform, MeshCollider>();

        void Start() {
            for(int i = 0; i < wheelAxies.Count; i++) {

                wheelPhysicMaterial.Add(wheelAxies[i].left, wheelAxies[i].left.GetComponent<MeshCollider>());
                wheelPhysicMaterial.Add(wheelAxies[i].right, wheelAxies[i].right.GetComponent<MeshCollider>());

                wheelPhysicMaterial[wheelAxies[i].left].material = new PhysicMaterial();
                wheelPhysicMaterial[wheelAxies[i].right].material = new PhysicMaterial();
            }
        }

        void Update() {
            Brake();
        }

        void FixedUpdate() {

            //wheelR.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));
            //wheelL.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));

            for(int i = 0; i < wheelAxies.Count; i++) {

                if(wheelAxies[i].steering) {
                    Turn(wheelAxies[i].left, wheelAxies[i].right);
                }

                if(wheelAxies[i].torque) {
                    Wheel(wheelAxies[i].left);
                    Wheel(wheelAxies[i].right);
                }
            }

            float speed = carBody.velocity.magnitude * 3.6f;
            text.text = "Real Speed: " + Mathf.RoundToInt(speed).ToString();

            if(to100) {
                if(speed > 1)
                    to100Time += Time.deltaTime;
                text1.text = "Time to 100 km/h: " + (Mathf.RoundToInt(to100Time * 100f) / 100f).ToString();
                if(speed >= 100)
                    to100 = false;
            }

        }

        void Wheel(Transform wheel) {
            if(isGrounded(wheel)) {

                float speed = carBody.velocity.magnitude * 3.6f;

                if(speed < maxSpeed) {

                    float pp = 0.0048f * coefficientOfAirResistance * frontalSurfaceOfTheCar * (speed * speed);

                    float ppMultiplier = pp / 1000f;
                    if(ppMultiplier < 1)
                        ppMultiplier = 1;

                    carBody.AddForceAtPosition(
                        carBody.transform.forward * 400000f * Input.GetAxis("Vertical") * Time.deltaTime * ppMultiplier,
                        new Vector3(wheel.position.x, wheel.position.y, wheel.position.z)
                    );
                }
            }
        }

        void Turn(Transform left, Transform right) {

            float angle = Input.GetAxis("Horizontal") * steeringAngle;

            left.localEulerAngles = new Vector3(
                    left.localEulerAngles.x,
                    angle,
                    left.localEulerAngles.z
                );
            right.localEulerAngles = new Vector3(
                right.localEulerAngles.x,
                angle,
                right.localEulerAngles.z
            );

            if(isGrounded(left) && isGrounded(right)) {
                
                float turn = Mathf.Lerp(angle * 8000, 0, carBody.velocity.magnitude / maxSpeed / 3f);

                carBody.MoveRotation(Quaternion.Euler(
                    carBody.transform.rotation.eulerAngles.x,
                    carBody.transform.rotation.eulerAngles.y + (turn / 5000f * Time.deltaTime),
                    carBody.transform.rotation.eulerAngles.z
                ));

                float steering = Mathf.RoundToInt(Vector3.Distance(carBody.velocity.normalized, transform.forward) * 1000f) * 2f;
                print(steering);
                print((steering <= steeringAngle + 10));

                if(!Input.GetKey(KeyCode.Space)) {
                    if(steering <= steeringAngle + 10 || Mathf.RoundToInt(carBody.velocity.magnitude) == 0) {

                        if(
                            Vector3.Distance(carBody.velocity.normalized, transform.forward)
                            <=
                            Vector3.Distance(carBody.velocity.normalized, -transform.forward
                        )) {

                            carBody.velocity = transform.forward * carBody.velocity.magnitude;
                        } else {
                            carBody.velocity = -transform.forward * carBody.velocity.magnitude;
                        }

                    }
                }
            }
        }

        void Brake() {
            if(Input.GetKey(KeyCode.Space)) {

                for(int i = 0; i < wheelAxies.Count; i++) {
                    if(wheelAxies[i].brake) {
                        ModifyFriction(wheelAxies[i], brakeFriction);
                    }
                }
            } else {

                for(int i = 0; i < wheelAxies.Count; i++) {
                    if(wheelAxies[i].brake) {
                        ModifyFriction(wheelAxies[i], clutchFriction);
                    }
                }
            }
        }

        void ModifyFriction(WheelAxle axle, float friction) {
            
            PhysicMaterial left = wheelPhysicMaterial[axle.left].material;
            PhysicMaterial right = wheelPhysicMaterial[axle.right].material;

            left.staticFriction = friction;
            left.dynamicFriction = friction;

            right.staticFriction = friction;
            right.dynamicFriction = friction;
        }

        bool isGrounded(Transform transform) {
            return Physics.Raycast(transform.position, -transform.right, 0.51f);
        }
    }
}
