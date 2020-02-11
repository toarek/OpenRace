using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenRace.Car {
    public class Controller : MonoBehaviour {
        public Transform wheelR;
        public Transform wheelL;
        public Transform fwheelR;
        public Transform fwheelL;

        public Rigidbody body;

        public Text text;
        public Text text1;

        public float maxSpeed = 100f;

        bool to100 = true;
        float to100Time = 0;

        float steeringAngle = 30f;

        [Header("Aerodynamics")]
        public float coefficientOfAirResistance = 0.4f;
        public float frontalSurfaceOfTheCar = 2.5f;

        private void FixedUpdate() {

            wheelR.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));
            wheelL.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));

            Wheel(wheelR);
            Wheel(wheelL);
            Turn(fwheelL, fwheelR);

            float speed = body.velocity.magnitude * 3.6f;
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

                float speed = body.velocity.magnitude * 3.6f;

                if(speed < maxSpeed) {

                    float pp = 0.0048f * coefficientOfAirResistance * frontalSurfaceOfTheCar * (speed * speed);
                    print(pp);

                    float ppMultiplier = pp / 1000f;
                    if(ppMultiplier < 1)
                        ppMultiplier = 1;

                    body.AddForceAtPosition(
                        body.transform.forward * 400000f * Input.GetAxis("Vertical") * Time.deltaTime * ppMultiplier,
                        new Vector3(wheel.position.x, 0, wheel.position.z)
                    );
                }
            }
        }

        void Turn(Transform left, Transform right) {

            if(isGrounded(left) && isGrounded(right)) {
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

                float turn = Mathf.Lerp(angle * 8000, 0, body.velocity.magnitude / maxSpeed / 3f);

                body.MoveRotation(Quaternion.Euler(
                    body.transform.rotation.eulerAngles.x,
                    body.transform.rotation.eulerAngles.y + (turn / 5000f * Time.deltaTime),
                    body.transform.rotation.eulerAngles.z
                ));

                if(body.velocity.magnitude * 3.6f > 15f)
                    body.velocity = transform.forward * body.velocity.magnitude;
            }
        }

        bool isGrounded(Transform transform) {
            return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
        }
    }
}
