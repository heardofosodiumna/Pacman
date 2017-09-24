using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

    float speed = 10.0f;
    Vector3 moveDir = Vector3.zero; 
	// Use this for initialization

	
	// Update is called once per frame
	void FixedUpdate () {
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
    /*
     *  public SteeringAgent TargetAgent;
        public float FleeRadius = 1.0f;
        public bool DrawGizmos = false;

        public override Vector2 GetVelocity()
        {
                //dist =   distact  from player to targer
            float distance = Vector3.Distance(transform.position, TargetAgent.transform.position);

            //if distance is <flee
            if (distance < FleeRadius)
            {
                // t is distance divied by max velocity
                float t = distance / TargetAgent.MaxVelocity;
                //point will be the Vec2 of target position + currentvel times t
                Vector2 targetPoint = (Vector2)TargetAgent.transform.position + TargetAgent.CurrentVelocity * t;
                //return the (target point - position).normlized * max veloxcity) - agent current velocity
                return -(((targetPoint - (Vector2)transform.position).normalized * agent.MaxVelocity) - agent.CurrentVelocity);
            }
            else
                return Vector2.zero;
        }

    */
}