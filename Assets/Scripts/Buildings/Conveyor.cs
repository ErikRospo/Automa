using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    // Animation
    public Animator animator;
    
    // Speed of the conveyor (temp)
    public bool isCorner;
    public float sizeAdjust;
    public bool isSetup = false;

    // Rotation for corners
    private RotationType corner;

    public void Start()
    {
        animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    // Called externally from building script, since corners need to override the
    // creation process. Still trying to figure out a way to make this an internal call
    public void Setup()
    {
        if (!isSetup)
        {
            isSetup = true;

            if (rotation == 0) SetupRotation();
            SetupPositions();
            CheckNearbyBuildings();

            transform.localScale = new Vector2(sizeAdjust, sizeAdjust);
        }
    }

    // Togles enabling a corner conveyor
    public void ToggleCorner(bool rotateUp)
    {
        SetupRotation();

        if (rotateUp)
        {
            if (rotation == RotationType.NORTH) corner = RotationType.WEST;
            else if (rotation == RotationType.EAST) corner = RotationType.NORTH;
            else if (rotation == RotationType.SOUTH) corner = RotationType.EAST;
            else if (rotation == RotationType.WEST) corner = RotationType.SOUTH;

            outputs[0].transform.localPosition = new Vector2(0, outputs[0].transform.localPosition.x);
            outputs[0].tile.localPosition = new Vector2(0, outputs[0].tile.localPosition.x);
        }
        else
        {
            if (rotation == RotationType.NORTH) corner = RotationType.EAST;
            else if (rotation == RotationType.EAST) corner = RotationType.SOUTH;
            else if (rotation == RotationType.SOUTH) corner = RotationType.WEST;
            else if (rotation == RotationType.WEST) corner = RotationType.NORTH;

            outputs[0].transform.localPosition = new Vector2(0, -outputs[0].transform.localPosition.x);
            outputs[0].tile.localPosition = new Vector2(0, -outputs[0].transform.localPosition.x);
        }

        isCorner = true;
        animator.enabled = !animator.enabled;
        if (!animator.enabled)
        {
            if (rotateUp) GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buildings/ConveyorTurnLeft");
            else GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buildings/ConveyorTurnRight");
        }
    }

    // Sets a conveyor bin
    public void SetInputBin(Entity entity)
    {
        inputs[0].bin = entity;
        acceptingEntities = false;
        UpdateBins();
    }

    public void SetOutputBin(Entity entity)
    {
        outputs[0].bin = entity;
        UpdateBins();
    }

    public override void UpdateBins()
    {
        // Checks the front container
        if (outputs[0].bin != null && outputs[0].target != null && outputs[0].target.acceptingEntities)
        {
            if (outputs[0].target.InputEntity(outputs[0].bin))
            {
                outputs[0].bin = null;
                outputs[0].reserved = false;
            }
        }
        
        // Check the back container
        if (inputs[0].bin != null && outputs[0].bin == null && !outputs[0].reserved)
        {
            if (isCorner) inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this, true);
            else inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[0], this, true);

            inputs[0].bin = null;
            acceptingEntities = true;
            outputs[0].reserved = true;
            inputs[0].reserved = false;

            if (inputs[0].target != null)
                inputs[0].target.UpdateBins();
        }
    }

    public void CornerCheck(Building building)
    {
        Debug.Log("Corner check | " + building.rotation + " = " + corner);

        // If the target is ahead of this conveyor, do the required check
        if (corner != 0 && building.rotation == corner)
        {
            SetOutputTarget(building);
            building.SetInputTarget(this);
            building.UpdateBins();
        }
    }

    public override void SetInputTarget(Building target)
    {
        inputs[0].target = target;
    }

    public override void SetOutputTarget(Building target)
    {
        outputs[0].target = target;
        UpdateBins();
    }

    public override bool InputEntity(Entity entity)
    {
        acceptingEntities = false;
        inputs[0].reserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].position, this);
        return true;
    }

    public override void ReceiveEntity(Entity entity)
    {
        SetInputBin(entity);
    }

    public override void OutputEntity(Entity entity)
    {
        if (entity.transform.position == transform.position)
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputs[0], this, true);
        else SetOutputBin(entity);
    }
}
