using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour {

    public enum ControllerType {keyboard, mouse, computer, none};
    public enum MovementDirection {horizontal, vertical}

    public ControllerType assignedControllerType;

    ControlKeyboardScript keyboardControl;
    ControlComputerScript computerControl;
    ControlMouseScript mouseControl;

    public int paddlePosition;
    public MovementDirection movementDirection;

    static float maxMovementDelta = .05f;
    bool blockedUp = false;
    bool blockedDown = false;
    bool blockedLeft = false;
    bool blockedRight = false;

    // Use this for initialization
    void Start () {
        assignedControllerType = ControllerType.none;
    }
	
	// Update is called once per frame
	void Update () {
		if (assignedControllerType == ControllerType.keyboard)
        {
            if (keyboardControl == null) keyboardControl = gameObject.AddComponent<ControlKeyboardScript>();
            keyboardControl.SetActivePaddle(this);
        }
        if (assignedControllerType == ControllerType.mouse)
        {
            if (mouseControl == null) mouseControl = gameObject.AddComponent<ControlMouseScript>();
            mouseControl.SetActivePaddle(this);
        }
        if (assignedControllerType == ControllerType.computer)
        {
            if (computerControl == null) computerControl = gameObject.AddComponent<ControlComputerScript>();
            computerControl.SetActivePaddle(this);
        }
    }

    public void DisablePaddle()
    {
        gameObject.SetActive(false);
    }

    public void SetControllerType(int controllerType)
    {
        if (keyboardControl != null) keyboardControl.SetInactive(); //because we are setting a new controller we set all current control scripts to inactive
        if (computerControl != null) computerControl.SetInactive();
        if (mouseControl != null) mouseControl.SetInactive();

        Debug.Log(name + " controller type set to int: " + controllerType);
        assignedControllerType = (ControllerType)controllerType; //casts the passed int to the enum
    }

    public void SetPaddlePosition(int position)
    {
        paddlePosition = position;
        if (position == 0 || position == 1) movementDirection = MovementDirection.vertical;
        else movementDirection = MovementDirection.horizontal;
    }

    //movement

    public void MoveVertical(float inputAmount)
    {
        if (blockedUp && inputAmount > 0)
        {
            return;
        }
        else if (blockedDown && inputAmount < 0)
        {
            return;
        }
        float movementAmount = inputAmount * Time.deltaTime;
        if (movementDirection == MovementDirection.horizontal) return;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, movementAmount), maxMovementDelta);
    }


    public void MoveHorizontal(float inputAmount)
    {
        if (blockedRight && inputAmount > 0)
        {
            return;
        }
        else if (blockedLeft && inputAmount < 0)
        {
            return;
        }
        float movementAmount = inputAmount * Time.deltaTime;
        if (movementDirection == MovementDirection.vertical) return;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(movementAmount, 0, 0), maxMovementDelta);
    }


    //collisions

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Walls"))
        {
            //Debug.Log("PADDLE enter " + col.gameObject.name + ", " + col.ClosestPoint(transform.position));
            if (movementDirection == MovementDirection.vertical)
            {
                if (col.ClosestPoint(transform.position).z < transform.position.z)
                {
                    //Debug.Log("hitting down wall");
                    blockedDown = true;
                }
                else
                {
                    //Debug.Log("hitting up wall");
                    blockedUp = true;
                }
            }

            if (movementDirection == MovementDirection.horizontal)
            {
                if (col.ClosestPoint(transform.position).x < transform.position.x)
                {
                    //Debug.Log("hitting left wall");
                    blockedLeft = true;
                }
                else
                {
                    //Debug.Log("hitting right wall");
                    blockedRight = true;
                }
            }
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name.Contains("Walls"))
        {
            //Debug.Log("PADDLE exit " + col.gameObject.name + ", " + col.ClosestPoint(transform.position));
            if (movementDirection == MovementDirection.vertical)
            {
                if (col.ClosestPoint(transform.position).z < transform.position.z)
                {
                    //Debug.Log("hitting down wall");
                    blockedDown = false;
                }
                else
                {
                    //Debug.Log("hitting up wall");
                    blockedUp = false;
                }
            }

            if (movementDirection == MovementDirection.horizontal)
            {
                if (col.ClosestPoint(transform.position).x < transform.position.x)
                {
                    //Debug.Log("hitting left wall");
                    blockedLeft = false;
                }
                else
                {
                    //Debug.Log("hitting right wall");
                    blockedRight = false;
                }
            }
        }
    }
}
