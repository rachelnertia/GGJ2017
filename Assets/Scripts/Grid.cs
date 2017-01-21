using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Linq;

public class GridInfo {
	public Vector2 pos;
	public bool full;
	
	public GridInfo (Vector2 p) {
		
		pos = p;
		full = false;
	}
}

public class Grid {
	
	public List<GridInfo> grid = new List<GridInfo>();
	
	// both starting values have to be lower than ending
	// public Grid (int columns, int rows, Vector2 startingPos, Vector2 endingPos) {
	public Grid (int crowdSize, Vector2 startingPos, Vector2 endingPos) {
		
		int rows = 4;
		int columns = crowdSize/rows;
		
		int totalPeople = columns * rows;
		
		Vector2 scale = new Vector2();
		scale.x = Mathf.Abs(startingPos.x - endingPos.x) / (columns - 1);
		scale.y = Mathf.Abs(startingPos.y - endingPos.y) / (rows - 1);
		
		for (int c = 0; c < columns; c++)
		{
			for (int r = 0; r < rows; r++)
			{
				float xPos = startingPos.x + (c * scale.x);
				float yPos = startingPos.y + (r * scale.y);
				
				Vector2 pos = new Vector2(xPos, yPos);
				
				grid.Add(new GridInfo(pos));
			}
		}
		
	}
	
	public Vector2 GetEmptySeat () {
		
		// Vector2 pos = Vector2.zero;
		
		// GridInfo info
		// .Where(n => n.full == false)
		// .Select(n => n)
		
		for (int i = 0; i < grid.Count; i++)
		{
			if (grid[i].full == false)
			{
				// info = grid[i];
				// break;
				
				grid[i].full = true;
				return grid[i].pos;
			}
		}
		
		// info.full = true;
		// return info.pos;
		
		return Vector2.zero;
	}
	
	// public void EmptySeat () {
		
	// }
}
	
	// Use this for initialization
	// void Start () {
		// griddo = new List<Vector2>();
		
		// for
		
		
		
		// for (int i = 0; i < totalPeople; i++)
		// {
			// var newCrowdMember = GameObject.Instantiate(crowdMemberPrefab) as GameObject;

			// // newCrowdMember.transform.position = griddo[i] + new Vector2(startingX, 0);
			// newCrowdMember.transform.position = griddo[i];
			// // grid[i].full = true;
		// }
	// }
	
	// public void MakeGrid (int c, int r, Vector2 s, Vector2 e) {
		
		// Vector2 scale = new Vector2();
		// scale.x = Mathf.Abs(s.x - e.x) / (c - 1);
		// scale.y = Mathf.Abs(s.y - e.y) / (r - 1);
		
		// Debug.Log(scale.y);
		
		// for (int i = 0; i < c; i++)
		// {
			// for (int j = 0; j < r; j++)
			// {
				// float xPos = s.x + (i * scale.x);
				// float yPos = s.y + (j * scale.y);
				
				// griddo.Add(new Vector2(xPos, yPos));
			// }
		// }
	// }
// }
