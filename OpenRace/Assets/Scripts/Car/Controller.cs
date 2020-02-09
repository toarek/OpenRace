using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Transform wheelR;
    public Transform wheelL;

    public Rigidbody body;

    public Text text;
    public Text text1;

    bool to100 = true;
    float to100Time = 0;

    float steeringAngle = 30f;

    private void FixedUpdate() {

        wheelR.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));
        wheelL.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));

        Wheel(wheelR);
        Wheel(wheelL);

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

            Vector3 direction0 = transform.forward;
            Vector3 direction1 = transform.forward;
            float directionPower = 1;

            float angle = Input.GetAxis("Horizontal") * steeringAngle;

            wheel.localEulerAngles = new Vector3(
                wheel.localEulerAngles.x,
                angle,
                wheel.localEulerAngles.z
            );

            print(angle);

            float rotation = angle;
            /*if(body.velocity.magnitude > 0f && body.velocity.magnitude < 5f) {
                rotation = Mathf.Lerp(0, angle, body.velocity.magnitude / 5f);
            } else if(body.velocity.magnitude > 0f) {
                rotation = Mathf.Lerp(0, angle, body.velocity.magnitude / 100f);
            }*/

            //body.transform.Rotate(0, rotation * Time.deltaTime, 0);

            rotation = Mathf.Lerp(angle, Input.GetAxis("Horizontal"), body.velocity.magnitude / 300f);

            body.AddForceAtPosition(
                body.transform.right * 10000f * rotation * Time.deltaTime,
                new Vector3(wheel.position.x, 0, wheel.position.z)
            );

            body.AddForceAtPosition(
                body.transform.forward * 400000f * Input.GetAxis("Vertical") * Time.deltaTime,
                new Vector3(wheel.position.x, 0, wheel.position.z)
            );
        }
    }

    bool isGrounded(Transform transform) {
        return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
    }
}
