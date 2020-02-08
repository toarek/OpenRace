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

    private void FixedUpdate() {

        wheelR.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));
        wheelL.Rotate(new Vector3(0, -Input.GetAxis("Vertical"), 0));

        if(isGrounded(wheelR))
            body.AddForceAtPosition(transform.forward * 400000f * Input.GetAxis("Vertical") * Time.deltaTime, wheelR.position);
        if(isGrounded(wheelL))
            body.AddForceAtPosition(transform.forward * 400000f * Input.GetAxis("Vertical") * Time.deltaTime, wheelL.position);

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

    bool isGrounded(Transform transform) {
        return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
    }
}
