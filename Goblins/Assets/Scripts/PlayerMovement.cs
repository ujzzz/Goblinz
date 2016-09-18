using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Animator anim;
	public Rigidbody rBody;
	private float moveX;
	private float moveZ;
	private Vector3 movement;
	[SerializeField]
	private float thrust;
	private Vector3 mousePoint;
	private bool pressed;
	[SerializeField]
	private float rotateSpeed;

	// Use this for initialization
	void Start () {
		// grabs the goblin animator and physics to manipulate later
		anim = GetComponent<Animator> ();
		rBody = GetComponent<Rigidbody> ();
		pressed = false;
	}

	// Update is called once per frame
	void Update () {
		// moves based on input
		PlayerMove ();

		// if left or right mouse button clicked then rotate player
		if (Input.GetMouseButton (0) || Input.GetMouseButton (1)) {
			PlayerRotate ();
			pressed = true;
		} else {
			pressed = false;
		}
	}

	// gotta run CPU-ntenstive physics shit in FixedUpdate, which isn't called every frame and less CPU intense
	void FixedUpdate () {
		// figure out where mouse click is on the map screen relative to the goblin
		Quaternion targetRotation = Quaternion.LookRotation (mousePoint - transform.position);	
		// then rotate the goblin in the direction of the click
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
		pressed = false;
	}

	private void PlayerMove () {
		// collect movement input
		moveX = Input.GetAxisRaw ("Horizontal");
		moveZ = Input.GetAxisRaw ("Vertical");
		movement = new Vector3 (moveX, 0f, moveZ);

		// if buttons are pressed, add physics to the player model to move them in that direction
		if (movement != Vector3.zero) {
			rBody.AddRelativeForce (movement * thrust);
			// and set the animator speed so that the animator picks it up and starts animating it
			anim.SetFloat("Speed", 10f);
		} else {
			anim.SetFloat("Speed", 0f);
		}
	}

	private void PlayerRotate () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			mousePoint = hit.point;
			mousePoint.y = 0;
		}
	}
}
