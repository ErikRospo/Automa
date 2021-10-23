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

    // Rotation for corners
    private RotationType corner;

    // Sets up the conveyor
    public override void ApplyOptions(int option)
    {
        // Setup rotation
        SetupRotation();

        // Setup corner if required
        if (option == 1) ToggleCorner(true);
        else if (option == 2) ToggleCorner(false);

        // Setup positions
        SetupPositions();
        CheckNearbyBuildings();

        transform.localScale = new Vector2(sizeAdjust, sizeAdjust);

        if (!isCorner)
            animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    // Togles enabling a corner conveyor
    public void ToggleCorner(bool rotateUp)
    {
        isCorner = true;

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
            outputs[0].tile.localPosition = new Vector2(0, -outputs[0].tile.localPosition.x);
        }

        //animator.enabled = !animator.enabled;

        if (rotateUp)
        {
            animator.SetBool("rotateUp", true);
            animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //GetComponent<SpriteRenderer>().sprite = SpritesManager.GetSprite("Corner Up");
            
        }
        else GetComponent<SpriteRenderer>().sprite = SpritesManager.GetSprite("Corner Down");
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
            else inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].position, this, true);

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
        // If the target is ahead of this conveyor, do the required check
        if (corner != 0 && building.rotation == corner)
        {
            SetOutputTarget(building);
            building.SetInputTarget(this);
            building.UpdateBins();
        }
    }

    public override bool SetInputTarget(Building target)
    {
        inputs[0].target = target;
        return true;
    }

    public override bool SetOutputTarget(Building target)
    {
        outputs[0].target = target;
        UpdateBins();
        return true;
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
        inputs[0].bin = entity;
        acceptingEntities = false;
        UpdateBins();
    }

    public override void OutputEntity(Entity entity)
    {
        if (entity.transform.position == transform.position)
        {
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].position, this, true);
        }
        else
        {
            outputs[0].bin = entity;
            UpdateBins();
        }
    }
}
