using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

    float speed = 10.0f;
    Vector3 moveDir = Vector3.zero; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CharacterController controller = GetComponent<CharacterController>();
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDir = transform.TransformDirection(moveDir);
        moveDir = moveDir * speed;
        controller.Move(moveDir * Time.deltaTime);
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "obj")
            Destroy(hit.gameObject);
    }

}
