using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo {
	public Vector2 pos;
	public bool full;
	
	public GridInfo (Vector2 p) {
		
		pos = p;
		full = false;
	}
}

public class Grid {
	
	// both starting values have to be lower than ending
	public Grid (int columns, int rows, Vector2 startingPos, Vector2 endingPos) {
		List<GridInfo> grid = new List<GridInfo>();
		
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
