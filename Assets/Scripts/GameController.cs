using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject crowdMemberPrefab;

    private enum GameStates {
        None,
        Warmup,
        InProgress,
        End
    }

    public void CreateCrowdMember(int team) {
        var newCrowdMember = GameObject.Instantiate(crowdMemberPrefab);
        var newCrowdMemberController = crowdMemberPrefab.GetComponent<CrowdMemberController>();
    }

    public void StartGame() {
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
