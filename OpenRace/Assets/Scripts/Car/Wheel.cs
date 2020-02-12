using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenRace.Car {
    public class Wheel : MonoBehaviour {

        bool grounded = false;

        public bool IsGrounded() {
            print("working!3");
            return grounded;
        }

        void OnCollisionEnter(Collision collision) {
            //if(collision.gameObject.tag == "Terrain") {
                print("working!2");
                grounded = true;
            //}
        }

        void OnCollisionExit(Collision collision) {
            //if(collision.gameObject.tag == "Terrain") {
                print("working!");
                grounded = false;
            //}
        }

    }
}
