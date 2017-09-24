using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

    public float speed;
    Vector3 moveDir = Vector3.zero; 

	
	// Update is called once per frame
	private void FixedUpdate () {
        if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = false;

        CharacterController controller = GetComponent<CharacterController>();
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDir = transform.TransformDirection(moveDir);
        moveDir = moveDir * speed;
        controller.Move(moveDir * Time.deltaTime);
    }
}