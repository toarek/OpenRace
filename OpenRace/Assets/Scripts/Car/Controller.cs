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

        public Transform backsteer;

        public Rigidbody body;

        public Text text;
        public Text text1;

        public float maxSpeed = 100f;

        bool to100 = true;
        float to100Time = 0;

        float steeringAngle = 30f;

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
                if(body.velocity.magnitude * 3.6f < maxSpeed) {

                    body.AddForceAtPosition(
                        body.transform.forward * 400000f * Input.GetAxis("Vertical") * Time.deltaTime,
                        new Vector3(wheel.position.x, 0, wheel.position.z)
                    );
                }
            }
        }

        void Turn(Transform left, Transform right) {

            Vector3 forceVector = Vector3.zero;
            bool forceVectorBool = false;

            bool leftIsGrounded = isGrounded(left);
            bool rightIsGrounded = isGrounded(right);

            if(leftIsGrounded && rightIsGrounded) {
                forceVector = Vector3.Lerp(left.position, right.position, 0.5f);
                forceVectorBool = true;

            } else if(leftIsGrounded) {
                forceVector = left.position;
                forceVectorBool = true;

            } else if(rightIsGrounded) {
                forceVector = right.position;
                forceVectorBool = true;

            }
            if(true) {
            //if(forceVectorBool) {
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

                float turn = Mathf.Lerp(angle * 8000 * Time.deltaTime, 0, body.velocity.magnitude / maxSpeed / 3f);

                /*body.AddForceAtPosition(
                    transform.right * turn,
                    forceVector
                );

                body.AddForceAtPosition(
                    -transform.right * turn,
                    backsteer.position
                );*/

                body.MoveRotation(Quaternion.Euler(
                    body.transform.rotation.eulerAngles.x,
                    body.transform.rotation.eulerAngles.y + (turn/1000f),
                    body.transform.rotation.eulerAngles.z
                ));
                //body.MoveRotation(Quaternion.Euler(transform.right));
            }
        }

        bool isGrounded(Transform transform) {
            return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
        }
    }
}
