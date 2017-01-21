using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject crowdMemberPrefab;

    private enum GameState {
        None,
        Warmup,
        InProgress,
        End
    }

    private GameState gameState = GameState.None;

    public void CreateCrowdMember(int team) {
        var newCrowdMember = GameObject.Instantiate(crowdMemberPrefab);
        var newCrowdMemberController = newCrowdMember.GetComponent<CrowdMemberController>();
        // TODO: Map the crowd member to a button on the correct player's controller.
        // TODO: Position the crowd member on the correct player's side of the screen.
    }

    public void StartGame() {
        // Do cleanup, if necessary.

        gameState = GameState.None;

        // TODO: Set up players

        // TODO: Generate crowd members

        // TODO: Go into 'start of game' mode, in which players can warm up before starting.
    }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
