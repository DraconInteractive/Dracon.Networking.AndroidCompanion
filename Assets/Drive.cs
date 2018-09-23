using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Drive : MonoBehaviour {

	public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    void Update() {
        float translation = 0;
        float rotation = 0;
        if (NetworkServerUI.h > 0 || NetworkServerUI.v > 0) {
            translation = NetworkServerUI.v * speed;
            rotation = NetworkServerUI.h * rotationSpeed;
        } else {
            translation = Input.GetAxis("Vertical") * speed;
            rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        }
        
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}

