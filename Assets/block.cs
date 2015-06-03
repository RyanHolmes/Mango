//------------------------------------------------------------------------------
// This is the basic block Object.
//------------------------------------------------------------------------------
using System;

public class Block:IComparable<Block>,IEquatable<Block>
{
	public int x;
	public int y;
	public int z;
	public Block[] neighbors = new Block[6];
	private int _neighborCount;
	public Block (int newx, int newy, int newz, Block newblock, int direction)
	{
		x = newx;
		y = newy;
		z = newz;
		neighbors[direction] = newblock;
		_neighborCount = 1;
	}
	public Block (int newx, int newy, int newz)
	{
		x = newx;
		y = newy;
		z = newz;
		_neighborCount = 0;
	}
	public Block (float newx, float newy, float newz){
		x = (int)newx;
		y = (int)newy;
		z = (int)newz;
		_neighborCount = 0;
	}
	public Block(Block block)
	{
		x = block.x;
		y = block.y;
		z = block.z;
		neighbors = block.neighbors;
		_neighborCount = 0;
		for (int i =0; i< neighbors.Length; i++) {
			if(neighbors[i] != null) _neighborCount ++;
		}
	}
	public Block(){
	}

	public bool isEmpty(){

		return _neighborCount == 0;
	}

	/* Make new block from block in x direction;
	 * 
	 **/
	public Block GenerateNewBlock(int direction){
		Block newBlock = new Block (this.x, this.y, this.z, this, DirectionReverse (direction));
		newBlock.moveDirection (direction);
		return newBlock;
	}
	public static int DirectionReverse(int direction){
			return (direction + 3) % 6;
	}
	/*	DIRECTIONS
	 * 1 = x+
	 * 2 = z+
	 * 3 = y+
	 * 4 = x-
	 * 5 = z-
	 * 6 = y-
	 **/
	public void moveDirection(int direction){
		switch (direction) {
			case 0:
				this.x += 1;
				break;
			case 1:
				this.z += 1;
				break;
			case 2:
				this.y += 1;
				break;
			case 3:
				this.x -= 1;
				break;
			case 4:
				this.z -= 1;
				break;
			case 5:
				this.y -= 1;
				break;
		}
	}
	public void addNeighbor(Block block, int direction){
		neighbors[direction] = block;
		_neighborCount++;
	}
	public void removeNeighbor(int direction){
		neighbors [direction] = null;
		_neighborCount--;
	}
	public int CompareTo(Block other)
	{
		if(other == null)
		{
			return 1;
		}
		if (other.x != this.x) {
			return (int)(other.x - this.x);
		} else if(other.y != this.y){
			return (int)(other.y - this.y);
		} else if(other.z != this.z){
			return (int)(other.z - this.z);
		}
		return 0;
	}
	public Boolean Equals(Block other){
		if (other == null)
			return this == null;
		return ((this.x == other.x && this.y == other.y && this.z == other.z));
	}
	public override String ToString(){
		return "x:" + x + "y:" + y + "z:" + z;
	}
}
