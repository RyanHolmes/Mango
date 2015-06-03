using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BlockMap
{
	private List<Block> adjacencyMatrix;
	private List<Block> blockList;
	private BlockComparer bc;
	private BlockMap ()
	{
		bc = new BlockComparer ();
		blockList = new List<Block>();
		adjacencyMatrix = new List<Block>();
	}
	private static BlockMap instance;
	public static BlockMap getInstance(){
		if (instance == null) {
			instance = new BlockMap ();
		}
		return instance;
	}
	public void AddNewBlockOfType(float x, float y, float z, String blockToPlace){
		Block point = new Block ((int)x, (int)y, (int)z); 
		AddNewBlockOfType (point,blockToPlace);
		return;
	}
	public void AddNewBlockOfType(Vector3 point, String blockToPlace){
		//block created from a spot on the ground
		Block newPoint = new Block ((int)point.x, (int)point.y, (int)point.z); 
		AddNewBlockOfType (newPoint,blockToPlace);
		return;
	}
	public void AddNewBlockOfType(Block point, String type){
		adjacencyMatrix.Sort (bc);
		int index = adjacencyMatrix.BinarySearch(point,bc);
		Debug.Log (index);
		if (index >= 0) {
			Debug.Log ("REMOVING HAPPENED");
			point = adjacencyMatrix[index];
			adjacencyMatrix.RemoveAt(index);
		}
		index = blockList.BinarySearch (point, bc);
		if (index >= 0) {
			return;
		} else {
			Debug.Log(blockList.ToString());
			Debug.Log(point.ToString());
		}
		switch (type) {
		case "BLOCK_STANDARD":
			Add(point, new BasicBlock(point));
			break;
		default:
			break;
		}
	}
	private void Add(Block point, BasicBlock blockToPlace){

		for(int i=0; i<point.neighbors.Length;i++){
			if(blockToPlace.neighbors[i] == null){
				Block newAdjacency = blockToPlace.GenerateNewBlock(i);
				int index = adjacencyMatrix.BinarySearch(newAdjacency,bc);
				if(index <0){
					adjacencyMatrix.Add(newAdjacency);
				} else {
					adjacencyMatrix[index].addNeighbor(blockToPlace,Block.DirectionReverse(i));
					newAdjacency = adjacencyMatrix[index];
				}
				blockToPlace.addNeighbor(newAdjacency,i);
			}
		}
		blockList.Add (blockToPlace);
		blockList.Sort (bc);
		adjacencyMatrix.Sort (bc);
		return;
	}
	public void Delete(Vector3 point){
		int index = blockList.BinarySearch (new Block((int)point.x,(int)point.y,(int)point.z), bc);
		if (index < 0)
			return;
		BasicBlock selectedBlock = (BasicBlock) blockList[index];
		selectedBlock.Destroy ();
		//GameObject.Destroy (selectedBlock.block);
		//delete all applicable AdjacencyBlocks;
		for (int i=0; i<selectedBlock.neighbors.Length; i++) {
			selectedBlock.neighbors[i].removeNeighbor(Block.DirectionReverse(i));
			if(selectedBlock.neighbors[i].isEmpty()){ 
				adjacencyMatrix.Remove(selectedBlock.neighbors[i]);
			} else {
				if(selectedBlock.neighbors[i].ToString() == "test"){
					Block newAdjacency = selectedBlock.neighbors[i].GenerateNewBlock(Block.DirectionReverse(i));
					selectedBlock.neighbors[i].addNeighbor(newAdjacency,Block.DirectionReverse(i));
					adjacencyMatrix.Add(newAdjacency);
					adjacencyMatrix.Sort (bc);
				}
			}
		}
		blockList.RemoveAt (index);
	}
}

