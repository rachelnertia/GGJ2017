using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberController : MonoBehaviour {

    private bool handsUp;
    public float jumpHeight;
    public Vector2 memberPosition; 
    public bool AreHandsUp() {
        return handsUp;
    }
	
	public Grid grid;

    public GenericInput input;

    public Sprite[] frames = new Sprite[2];

    public bool isInCurrentlySelectedGroup = false;

	// Use this for initialization
	void Start () {

    }

    public float putHandsDownCooldown = 1.0f;

    private float handsUpTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		if (GameController.gameState == GameController.GameState.InProgress)
		{
			if (isInCurrentlySelectedGroup && input.IsActive()) {
				if (!handsUp) {
					// Go to hands up frame.
					gameObject.GetComponent<SpriteRenderer>().sprite = frames[1];
					transform.position = new Vector2(transform.position.x, transform.position.y + jumpHeight);


					handsUp = true;
				}

				handsUpTime = 0.0f;
			} else {
				

				handsUpTime += Time.deltaTime;

				if (handsUp) {
					if (handsUpTime > putHandsDownCooldown / grid.timeMultiplier) {
						// Go to hands down frame.
						gameObject.GetComponent<SpriteRenderer>().sprite = frames[0];
						transform.position = memberPosition;

						handsUp = false;
					}
				}
			}
		}
	}
}
