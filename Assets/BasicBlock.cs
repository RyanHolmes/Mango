//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by() a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may() cause incorrect behavior and will be lost if
//     the code is regenerated.
using System;
using UnityEngine;

public class BasicBlock :Block
{
	public static string tag = "BLOCK_STANDARD";
	private GameObject gameBlock;
	public BasicBlock (Vector3 position):base((int)position.x,(int)position.y,(int)position.z)
	{

		gameBlock = (GameObject)GameObject.Instantiate ((GameObject)Resources.Load("block",typeof (GameObject)) , position, Quaternion.identity);
		gameBlock.tag = tag;
	}
	public BasicBlock (Block point):base(point)
	{
		gameBlock = (GameObject)GameObject.Instantiate ((GameObject)Resources.Load("block",typeof (GameObject)), new Vector3(x,y,z), Quaternion.identity);
		gameBlock.tag = tag;
	}
	public void AddBlock(Block other,int direction){
		this.neighbors [direction] = other;
		other.neighbors[Block.DirectionReverse(direction)] = this;
	}
	public void Destroy(){
		GameObject.Destroy (gameBlock);
	}
	public override string ToString ()
	{
		return "test";
	}
}

