using UnityEngine;
using System.Collections;

public class shootArrow : MonoBehaviour {

	[SerializeField]
	GameObject arrowPrefab;
	private Vector3 arrowOrigin;
	private Vector3 arrowTerminal;
	[SerializeField]
	private float arrowSpeed;
	private float maximumArrowForce = 50f;

	float lerpTime = 1f;
	float currentLerpTime;

	// Use this for initialization
	void Start () {
		// loads the arrow as a resource to be used later
		arrowPrefab = Resources.Load ("Arrow") as GameObject;
		// sets athe minimum amount of time the goblin needs to pull the bow back to aim it and shit.. so you don't insta-fire
		currentLerpTime = -2f;
	}
	
	// Update is called once per frame
	void Update () {
		// checks if right mouse button is pressed
		SetArrow ();
		// check if right mouse button is release and fire that shit
		ReleaseArrow ();
	}

	private void SetArrow () {
		if (Input.GetMouseButtonDown (1)) {
			// if button is pressed, find where mouse is pointing to 
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			// figure out where exactly on game map that line intersects and gets the exact location
			if (Physics.Raycast (ray, out hit)) {
				arrowOrigin = hit.point;
			}
		}

		// depending on how long the dude is holding it for, add more strength to the shot
		if (Input.GetMouseButton (1)) {
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime)
				currentLerpTime = lerpTime;

			// Lerp the arrow force the longer the player holds down the button.
			float perc = currentLerpTime / lerpTime;
			arrowSpeed = Mathf.Lerp (0f, maximumArrowForce, perc);
			print (arrowSpeed);
		}
	}

	private void ReleaseArrow() {
		if (Input.GetMouseButtonUp (1)) {
			// to be honest this part is not used because for now I'm using how long of TIME he draws bow back rather than how far he pulls it back on the game screen
			// but if it is used, this would record where the mouse was on the game screen when the right mouse button was release
			Ray rayEnd = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitEnd;
			if (Physics.Raycast (rayEnd, out hitEnd)) {
				arrowTerminal = hitEnd.point;
			
				// sets arrow target to where mouse was originally clicked
				arrowOrigin = arrowOrigin - transform.position;
				// creates the arrow right inside the goblin (need to adjust once model is ready so it spawns inside the bow basically)
				GameObject arrow = Instantiate (arrowPrefab, transform.position, transform.rotation) as GameObject;
				// shoots that shit by applying a physics force that is based on how long the mouse was held down
				arrow.GetComponent<Rigidbody> ().AddForce (arrowOrigin * arrowSpeed);

				//sets arrow speed back to 0 for the next bow
				arrowSpeed = 0f;
				currentLerpTime = 0f;
			}
		}
	}
		
}