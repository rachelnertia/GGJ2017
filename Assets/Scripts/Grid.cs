using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo {
	public Vector2 pos;
	public bool full;
	public CrowdMemberController crowdMember;
	
	public GridInfo (Vector2 p) {
		
		pos = p;
		full = false;
	}
}

public class Seat {
	
	public bool full;
	public CrowdMemberController crowdMember;
	
	public Seat () {
		full = false;
	}
}

public class Grid {
	
	public int currentColumn;
	public float scoreMultiplier = 1;
	public float scoreMultiplierIncrease = 1;
	
	public float timeMultiplier = 1;
	public float timeMultiplierIncrease = 0.1f;
	
	int rowCount = 4;
	Seat[,] seatColumns;
	
	public GameObject currentColumnPin;
	public int teamNo;
	
	public List<GridInfo> grid = new List<GridInfo>();
	
	// public Vector2 scale;
	// public Vector2 startPos;
	private float[] columnPositions;
	
	// private float score;
	
	// both starting values have to be lower than ending
	public Grid (int crowdSize, Vector2 startingPos, Vector2 endingPos) {
		
		// startPos = startingPos;
		
		int rows = rowCount;
		int columns = crowdSize/rows;
		
		columnPositions = new float[columns];
		
		seatColumns = new Seat[columns,rows];
		for(int i = 0; i < columns; i++) 
		{
			for(int j = 0; j < rows; j++) 
			{
				seatColumns[i,j] = new Seat();
			}
		}
		
		Vector2 scale = new Vector2();
		scale.x = Mathf.Abs(startingPos.x - endingPos.x) / (columns - 1);
		scale.y = Mathf.Abs(startingPos.y - endingPos.y) / (rows - 1);
		
		for (int c = 0; c < columns; c++)
		{
			float xPos = startingPos.x + (c * scale.x);
			columnPositions[c] = xPos;
		
			for (int r = 0; r < rows; r++)
			{
				// float xPos = startingPos.x + (c * scale.x);
				float yPos = startingPos.y + (r * scale.y);
				
				Vector2 pos = new Vector2(xPos, yPos);
				
				grid.Add(new GridInfo(pos));
			}
		}
	}
	
	public Vector2 FillEmptySeat (CrowdMemberController currentCrowdMember) {
		
		for (int i = 0; i < grid.Count; i++)
		{
			if (grid[i].full == false)
			{
				grid[i].full = true;
				grid[i].crowdMember = currentCrowdMember;
				
				FillSeat(i, currentCrowdMember);
				return grid[i].pos;
			}
		}
		
		return Vector2.zero;
	}
	
	void FillSeat (int i, CrowdMemberController currentCrowdMember) {
		int c = (int)Mathf.Floor(i / 4);
		int r = i % 4;
		
		seatColumns[c,r].full = true;
		seatColumns[c,r].crowdMember = currentCrowdMember;
	}
	
	public void LightUpColumn (int c) {
		
		if (c >= seatColumns.GetLength(0) - 1)
		{
			c = c % seatColumns.GetLength(0);
		}
	
		for (int i = 0; i < rowCount; i++)
		{
			if (seatColumns[c,i].full == true)
			{
				seatColumns[c,i].crowdMember.gameObject.GetComponent<SpriteRenderer>().flipY = true;
			}
		}
		
		if (c > 0)
		{
			LightDownColumn(c-1);
		}else{
			LightDownColumn(seatColumns.GetLength(0)-1);
		}
	}
	
	public void LightDownColumn (int c) {
		for (int i = 0; i < rowCount; i++)
		{
			if (seatColumns[c,i].full == true)
			{
				seatColumns[c,i].crowdMember.gameObject.GetComponent<SpriteRenderer>().flipY = false;
			}
		}
	}
	
	public void CheckWave () {
		
		currentColumnPin.transform.position = new Vector2(columnPositions[currentColumn], currentColumnPin.transform.position.y);
		
		int c = currentColumn;
		bool done = true;
		
		for (int i = 0; i < rowCount; i++)
		{
			if (seatColumns[c,i].full == true)
			{
				if (seatColumns[c,i].crowdMember.AreHandsUp() == false)
				{
					done = false;
				}
			}
		}
		
		// MEGA DEBUG
		// if (Input.GetKeyDown("Alpha"+currentColumn);
		if (Input.GetKeyDown("q") && teamNo == 0)
		{
			done = true;
		}
		if (Input.GetKeyDown("m") && teamNo == 1)
		{
			done = true;
		}
		
		if (done == true)
		{
			// Debug.Log("DONE");
			currentColumn++;
			if (currentColumn == seatColumns.GetLength(0))
			{
				currentColumn = 0;
				
				scoreMultiplier += scoreMultiplierIncrease;
				timeMultiplier += timeMultiplierIncrease;
			}
			
			// score += 1 * multiplier;
			float score = 1 * scoreMultiplier;
			GameObject.Find("GameObject").GetComponent<Score>().UpdateScore(teamNo, score);
		}
	}
}
