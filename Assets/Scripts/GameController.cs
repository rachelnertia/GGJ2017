using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public List<GameObject> crowdMembers = new List<GameObject>();

    public List<KeyCode> keys = new List<KeyCode>();

    public HashSet<KeyCode> keysThatHaveBeenClaimed = new HashSet<KeyCode>();

    public Player() {
        keys = new List<KeyCode>();
        keys.Add(KeyCode.Joystick1Button0);
        keys.Add(KeyCode.Joystick1Button1);
        keys.Add(KeyCode.Joystick1Button2);
        keys.Add(KeyCode.Joystick1Button3);
        keys.Add(KeyCode.Joystick1Button4);
        keys.Add(KeyCode.Joystick1Button5);
        keys.Add(KeyCode.Joystick1Button6);
        keys.Add(KeyCode.Joystick1Button7);
        keys.Add(KeyCode.Joystick1Button8);
        keys.Add(KeyCode.Joystick1Button9);
        keys.Add(KeyCode.Joystick1Button10);
    }
}

public class GameController : MonoBehaviour {

    public GameObject crowdMemberPrefab;

    private enum GameState {
        None,
        Warmup,
        InProgress,
        End
    }

    private GameState gameState = GameState.None;

    

    public Player[] players = new Player[2];

    public void CreateCrowdMember(Player player) {
        var newCrowdMember = GameObject.Instantiate(crowdMemberPrefab) as GameObject;

        newCrowdMember.transform.position = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        var newCrowdMemberController = newCrowdMember.GetComponent<CrowdMemberController>();
        
        KeyCode kc = player.keys[Random.Range(0, player.keys.Count)];

        newCrowdMemberController.keyCode = kc;

        player.crowdMembers.Add(newCrowdMember);
    }

    public void StartGame() {
        gameState = GameState.None;
        
        players[0] = new Player();
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);
        CreateCrowdMember(players[0]);

        gameState = GameState.Warmup;
    }

    // Use this for initialization
	void Start () {
        players = new Player[2];

        StartGame();
	}
	
	// Update is called once per frame
	void Update () {

    }
}
