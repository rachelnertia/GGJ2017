using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInput {
    public enum Type {
        Button,
        Axis,
        KeyCode
    }

    private Type type;

    private string buttonOrAxisName;
    private KeyCode keyCode;

    private float axisThreshold;
    private bool axisNegative;

    public GenericInput(string buttonName) {
        this.type = Type.Button;
        this.buttonOrAxisName = buttonName;
    }

    public GenericInput(KeyCode keyCode) {
        this.type = Type.KeyCode;
        this.keyCode = keyCode;
    }

    public GenericInput(string axisName, float threshold) {
        this.type = Type.Axis;
        this.buttonOrAxisName = axisName;
        this.axisThreshold = threshold;
        this.axisNegative = threshold < 0.0f;

    }

    public bool IsActive() {
        switch (type) {
            case Type.Button:
                return Input.GetButton(buttonOrAxisName);
            case Type.Axis:
                return axisNegative ? 
                    Input.GetAxis(buttonOrAxisName) < axisThreshold : 
                    Input.GetAxis(buttonOrAxisName) > axisThreshold;
            case Type.KeyCode:
                return Input.GetKey(keyCode);
        }

        return false;
    }
}

public class Player {
    public List<GameObject> crowdMembers = new List<GameObject>();

    public List<GenericInput> availableInputs = new List<GenericInput>();

    //public HashSet<KeyCode> keysThatHaveBeenClaimed = new HashSet<KeyCode>();
    
    public Player() {
        availableInputs = new List<GenericInput>();

        
    }

    public Grid crowdGrid;
}

public class GameController : MonoBehaviour {

    public int crowdSize = 32;

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

        newCrowdMember.transform.position = player.crowdGrid.GetEmptySeat();

        var newCrowdMemberController = newCrowdMember.GetComponent<CrowdMemberController>();
        
        GenericInput i = player.availableInputs[Random.Range(0, player.availableInputs.Count)];

        newCrowdMemberController.input = i;

        player.crowdMembers.Add(newCrowdMember);
    }

    public void StartGame() {
        gameState = GameState.None;

        players = new Player[2];

        {
            players[0] = new Player();

            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button0));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button1));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button2));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button3));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button4));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button5));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button6));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button7));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button8));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button9));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Horizontal", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Horizontal", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Vertical", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Vertical", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Vertical", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Vertical", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Horizontal", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Horizontal", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Horizontal", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Horizontal", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Vertical", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Vertical", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Left Trigger", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Trigger", 0.75f));

            players[0].crowdGrid = new Grid(crowdSize, new Vector2(-7.0f, 2.0f), new Vector2(7.0f, -1.0f));

            for (int i = 0; i < crowdSize; ++i) {
                CreateCrowdMember(players[0]);
            }
        }

        {
            players[1] = new Player();

            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button0));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button1));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button2));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button3));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button4));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button5));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button6));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button7));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button8));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button9));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Horizontal", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Horizontal", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Vertical", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Vertical", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Vertical", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Vertical", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Horizontal", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Horizontal", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Horizontal", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Horizontal", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Vertical", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Vertical", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Trigger", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Trigger", 0.75f));

            players[1].crowdGrid = new Grid(crowdSize, new Vector2(-7.0f, -3.0f), new Vector2(7.0f, -6.0f));

            for (int i = 0; i < crowdSize; ++i) {
                CreateCrowdMember(players[1]);
            }
        }

        gameState = GameState.Warmup;
    }

    // Use this for initialization
	void Start () {

        StartGame();
	}
	
	// Update is called once per frame
	void Update () {

    }
}
