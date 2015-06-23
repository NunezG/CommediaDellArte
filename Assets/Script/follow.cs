using UnityEngine;
using System.Collections;


public class follow : MonoBehaviour {


	public GameObject target;
	public Direction offsetType = Direction.World;
	public Vector3 offset;	
	public float smoothTime = 0.3F;
	public bool smoothDamp = false, followRotation = true, followPosition = true,
	positionX = true,positionY = true, positionZ = true ,
	rotationX = true,rotationY = true, rotationZ = true ;
	
	private float xVelocity = 0.0F,yVelocity = 0.0F,zVelocity = 0.0F;
	private Vector3 currentOffset, nextPosition,nextRotation;

	public enum Direction{None, World, Local};

	// Use this for initialization
	void Start () {
		nextPosition.x = this.transform.position.x;
		nextPosition.y = this.transform.position.y;
		nextPosition.z = this.transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {}

	void LateUpdate(){

		computeOffset ();

		if(followPosition) {
			if (smoothDamp) {

				if(positionX) nextPosition.x = Mathf.SmoothDamp (this.transform.position.x, target.transform.position.x + currentOffset.x, ref xVelocity, smoothTime);
				if(positionY) nextPosition.y = Mathf.SmoothDamp (this.transform.position.y, target.transform.position.y + currentOffset.y, ref yVelocity, smoothTime);
				if(positionZ) nextPosition.z = Mathf.SmoothDamp (this.transform.position.z, target.transform.position.z + currentOffset.z, ref zVelocity, smoothTime);
				this.transform.position = nextPosition;
			} 
			else{
				if(positionX) nextPosition.x = target.transform.position.x;
                if(positionY) nextPosition.y = target.transform.position.y;
				if(positionZ) nextPosition.z = target.transform.position.z;
				this.transform.position =   nextPosition +  currentOffset;
			}
		}
		if (followRotation) {
			if(rotationX) nextRotation.x = target.transform.rotation.eulerAngles.x;
			if(rotationY) nextRotation.y = target.transform.rotation.eulerAngles.y;
			if(rotationZ) nextRotation.z = target.transform.rotation.eulerAngles.z;

			this.transform.eulerAngles = nextRotation;
		}
	}


	void computeOffset(){
		switch (offsetType) {	
			case( Direction.None):
				currentOffset = new Vector3(0,0,0);
				break;
			case(Direction.World):
				currentOffset = offset;
				break;
			case(Direction.Local):
				currentOffset = new Vector3(0,0,0);
				currentOffset += target.transform.forward * offset.x;
				currentOffset += target.transform.right * offset.z;
				currentOffset += target.transform.up * offset.z;
				break;
		}
	}

}
