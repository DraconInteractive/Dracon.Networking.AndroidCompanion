using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Drive : MonoBehaviour {

	public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    void Update() {
        float translation = NetworkServerUI.v * speed;
        float rotation = NetworkServerUI.h * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}

