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

public class Utilities {

    public static void Shuffle<T>(List<T> list) {
        for (var i = 0; i < list.Count; i++)
            Swap(list, i, Random.Range(i, list.Count));
    }

    public static void Swap<T>(List<T> list, int i, int j) {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

}

public class Player {
    //public List<GameObject> crowdMembers = new List<GameObject>();

    public List<GenericInput> availableInputs = new List<GenericInput>();

    //public HashSet<KeyCode> keysThatHaveBeenClaimed = new HashSet<KeyCode>();

    public GameObject selectionRect;

    //public GameObject waveFrontMarker;

    public GameController gameController;

    public Player() {
        availableInputs = new List<GenericInput>();

        crowdMemberGroups = new List<List<GameObject>>();
        crowdMemberGroups.Add(new List<GameObject>());
    }

    public Grid crowdGrid;

    public List<List<GameObject>> crowdMemberGroups = new List<List<GameObject>>();

    public GenericInput groupButton1;
    public GenericInput groupButton2;
    public GenericInput groupButton3;
    public GenericInput groupButton4;

    int currentlySelectedGroup = 0;

    public List<List<GenericInput>> unclaimedInputsForGroups = new List<List<GenericInput>>();

    public void InitialisedUnclaimedInputForGroup(int groupNum) {
        foreach (var input in availableInputs) {
            unclaimedInputsForGroups[groupNum].Add(input);
        }

        Utilities.Shuffle(unclaimedInputsForGroups[groupNum]);
    }

    public GenericInput GetUnclaimedInputForGroup(int groupNum) {
        Debug.Assert(unclaimedInputsForGroups[groupNum].Count > 0);
        var input = unclaimedInputsForGroups[groupNum][0];
        unclaimedInputsForGroups[groupNum].RemoveAt(0);
        return input;
    }

    public void OnWarmup() {
        int prevCurrentGroup = currentlySelectedGroup;

        currentlySelectedGroup = crowdGrid.currentColumn;

        //if (groupButton1.IsActive()) {
        //    currentlySelectedGroup = 1;
        //}
        //else if (groupButton2.IsActive()) {
        //    currentlySelectedGroup = 2;
        //} 
        //else if (groupButton3.IsActive()) {
        //    currentlySelectedGroup = 3;
        //} 
        //else if (groupButton4.IsActive()) {
        //    currentlySelectedGroup = 4;
        //}
        //else {
        //    currentlySelectedGroup = 0;
        //}

        if (currentlySelectedGroup != prevCurrentGroup) {
            if (currentlySelectedGroup >= crowdMemberGroups.Count) {
                Debug.Log("Invalid currently selected group " + currentlySelectedGroup.ToString() + 
                    ", going back to old selection " + prevCurrentGroup.ToString());

                currentlySelectedGroup = prevCurrentGroup;
            } else {
                Debug.Log("New selected group: " + currentlySelectedGroup.ToString());
                // Deactivate members of previous group
                foreach (var crowdMember in crowdMemberGroups[prevCurrentGroup]) {
                    crowdMember.GetComponent<CrowdMemberController>().isInCurrentlySelectedGroup = false;
                }

                ActivateGroup(currentlySelectedGroup);
            }
        }

        crowdGrid.CheckWave();
    }

    public void ActivateGroup(int groupIndex) {
        float furthestLeft = crowdMemberGroups[groupIndex][0].transform.position.x;
        float furthestUp = crowdMemberGroups[groupIndex][0].transform.position.y;

        // Active members of the new group
        foreach (var crowdMember in crowdMemberGroups[groupIndex]) {
            crowdMember.GetComponent<CrowdMemberController>().isInCurrentlySelectedGroup = true;
            furthestLeft = Mathf.Min(furthestLeft, crowdMember.transform.position.x);
            furthestUp = Mathf.Max(furthestUp, crowdMember.transform.position.y);
        }

        Vector2 offset = gameController.selectionRectTopLeftOffset;

        selectionRect.transform.position = new Vector2(furthestLeft + offset.x, furthestUp + offset.y);
    }
}

public class GameController : MonoBehaviour {

    public int crowdSize = 100;
    public int groupSize = 20;

    public GameObject crowdMemberPrefab;

    public GameObject selectionRectPrefab;
    public GameObject waveFrontMarkerPrefab;

    public Vector2 selectionRectTopLeftOffset = new Vector2(0, 0);

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

        var newCrowdMemberController = newCrowdMember.GetComponent<CrowdMemberController>();

        newCrowdMember.transform.position = player.crowdGrid.FillEmptySeat(newCrowdMemberController);   
        newCrowdMember.transform.position = new Vector2 (newCrowdMember.transform.position.x + Random.Range(-0.2f, 0.2f), newCrowdMember.transform.position.y);
        newCrowdMemberController.memberPosition = newCrowdMember.transform.position;
        {
            int spriteIndex = Random.Range(0, idleFrames.Count);

            var ctrl = newCrowdMember.GetComponent<CrowdMemberController>();

            ctrl.frames = new Sprite[2];
            ctrl.frames[0] = idleFrames[spriteIndex];
            ctrl.frames[1] = armsUpFrames[spriteIndex];

            newCrowdMember.GetComponent<SpriteRenderer>().sprite = ctrl.frames[0];
        }

        //player.crowdMembers.Add(newCrowdMember);
        int groupWithSpace = 0;

        // Find an group.
        for (int i = 0; i < player.crowdMemberGroups.Count; ++i) {
            if (player.crowdMemberGroups[i].Count < groupSize) {
                groupWithSpace = i;
                break;
            } else {
                groupWithSpace++;
            }
        }

        if (groupWithSpace < player.crowdMemberGroups.Count) {
            // Put the new crowd member into this group.
            Debug.Log("Adding crowd member to existing group");
            player.crowdMemberGroups[groupWithSpace].Add(newCrowdMember);
        } else {
            // Make a new group and add the crowd member to it.
            Debug.Log("Making new group");
            var newList = new List<GameObject>();
            newList.Add(newCrowdMember);
            player.crowdMemberGroups.Add(newList);

            player.unclaimedInputsForGroups.Add(new List<GenericInput>());
            player.InitialisedUnclaimedInputForGroup(groupWithSpace);
        }

        {
            //GenericInput i = player.availableInputs[Random.Range(0, player.availableInputs.Count)];

            GenericInput i = player.GetUnclaimedInputForGroup(groupWithSpace);

            newCrowdMemberController.input = i;
        }
    }

    public void StartGame() {
        gameState = GameState.None;

        players = new Player[2];

        {
            players[0] = new Player();
            players[0].gameController = this;

            players[0].selectionRect = GameObject.Instantiate(selectionRectPrefab);

            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button0));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button1));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button2));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button3));
            //players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button4));
            //players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button5));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button6));
            players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button7));
            //players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button8));
            //players[0].availableInputs.Add(new GenericInput(KeyCode.Joystick1Button9));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Horizontal", 0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Horizontal", -0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Vertical", 0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Vertical", -0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Vertical", 0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Vertical", -0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Horizontal", 0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Horizontal", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Horizontal", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Horizontal", -0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Vertical", 0.75f));
            players[0].availableInputs.Add(new GenericInput("Joystick 1 D-Pad Vertical", -0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Left Trigger", 0.75f));
            //players[0].availableInputs.Add(new GenericInput("Joystick 1 Right Trigger", 0.75f));

            players[0].groupButton1 = new GenericInput("Joystick 1 Left Trigger", 0.75f);
            players[0].groupButton2 = new GenericInput("Joystick 1 Right Trigger", 0.75f);
            players[0].groupButton3 = new GenericInput(KeyCode.Joystick1Button4); // Left bumper
            players[0].groupButton4 = new GenericInput(KeyCode.Joystick1Button5); // Right bumper

            players[0].unclaimedInputsForGroups.Add(new List<GenericInput>());
            players[0].InitialisedUnclaimedInputForGroup(0);

            players[0].crowdGrid = new Grid(crowdSize, new Vector2(-7.0f, 2.0f), new Vector2(7.0f, -1.0f));
            players[0].crowdGrid.currentColumnPin = GameObject.Instantiate(waveFrontMarkerPrefab);

            for (int i = 0; i < crowdSize; ++i) {
                CreateCrowdMember(players[0]);
            }

            players[0].ActivateGroup(0);

        }

        {
            players[1] = new Player();
            players[1].gameController = this;

            players[1].selectionRect = GameObject.Instantiate(selectionRectPrefab);

            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button0));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button1));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button2));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button3));
            //players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button4));
            //players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button5));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button6));
            players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button7));
            //players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button8));
            //players[1].availableInputs.Add(new GenericInput(KeyCode.Joystick2Button9));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Horizontal", 0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Horizontal", -0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Vertical", 0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Vertical", -0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Vertical", 0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Vertical", -0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Horizontal", 0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Horizontal", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Horizontal", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Horizontal", -0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Vertical", 0.75f));
            players[1].availableInputs.Add(new GenericInput("Joystick 2 D-Pad Vertical", -0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Left Trigger", 0.75f));
            //players[1].availableInputs.Add(new GenericInput("Joystick 2 Right Trigger", 0.75f));

            players[1].groupButton1 = new GenericInput("Joystick 2 Left Trigger", 0.75f);
            players[1].groupButton2 = new GenericInput("Joystick 2 Right Trigger", 0.75f);
            players[1].groupButton3 = new GenericInput(KeyCode.Joystick2Button4); // Left bumper
            players[1].groupButton4 = new GenericInput(KeyCode.Joystick2Button5); // Right bumper

            players[1].unclaimedInputsForGroups.Add(new List<GenericInput>());
            players[1].InitialisedUnclaimedInputForGroup(0);

            players[1].crowdGrid = new Grid(crowdSize, new Vector2(-7.0f, -3.0f), new Vector2(7.0f, -6.0f));
            players[1].crowdGrid.currentColumnPin = GameObject.Instantiate(waveFrontMarkerPrefab);


            for (int i = 0; i < crowdSize; ++i) {
                CreateCrowdMember(players[1]);
            }

            players[1].ActivateGroup(0);
        }

        gameState = GameState.Warmup;
    }

    public List<Sprite> idleFrames;
    public List<Sprite> armsUpFrames;

    // Use this for initialization
    void Start () {

        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
        switch (gameState) {
            case GameState.Warmup:
                players[0].OnWarmup();
                players[1].OnWarmup();

                break;
            case GameState.InProgress:

                break;
        }
    }

    //void 
}
