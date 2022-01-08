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

    // Sets up the conveyor
    public override void ApplyOptions(int option)
    {
        // Setup corner if required
        if (option == 1) ToggleCorner(true);
        else if (option == 2) ToggleCorner(false);

        // Setup positions
        SetupPositions();
        CheckNearbyBuildings();

        transform.localScale = new Vector2(sizeAdjust, sizeAdjust);

        if (!isCorner)
            animator.Play("Base Layer.Both", -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    // Togles enabling a corner conveyor
    public void ToggleCorner(bool rotateUp)
    {
        isCorner = true;
        outputs[0].RotatePosition(rotateUp);

        //animator.enabled = !animator.enabled;

        if (rotateUp)
        {
            animator.SetBool("Corner", true);
            animator.Play("Base Layer.Corner", -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else
        {
            animator.SetBool("Corner", true);
            animator.Play("Base Layer.Corner", -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }

    public override void UpdateBins()
    {
        // Checks the front container
        if (outputs[0].bin != null && outputs[0].building != null && outputs[0].building.acceptingEntities)
        {
            if (outputs[0].building.InputEntity(outputs[0].bin))
            {
                outputs[0].bin = null;
                outputs[0].reserved = false;
            }
        }
        
        // Check the back container
        if (inputs[0].bin != null && outputs[0].bin == null && !outputs[0].reserved)
        {
            if (isCorner) inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this, true);
            else inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].binPosition, this, true);

            inputs[0].bin = null;
            acceptingEntities = true;
            outputs[0].reserved = true;
            inputs[0].reserved = false;

            if (inputs[0].building != null)
                inputs[0].building.UpdateBins();
        }
    }

    public void CornerCheck(Building building)
    {
        /* If the target is ahead of this conveyor, do the required check
        if (CheckIOPositions(building.inputs) != -1)
        {
            SetOutputTarget(building);
            building.SetInputTarget(this);
            building.UpdateBins();
        }
        */
    }

    public override bool InputEntity(ItemEntity entity)
    {
        acceptingEntities = false;
        inputs[0].reserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].binPosition, this);
        return true;
    }

    public override void ReceiveEntity(ItemEntity entity)
    {
        inputs[0].bin = entity;
        acceptingEntities = false;
        UpdateBins();
    }

    public override void OutputEntity(ItemEntity entity)
    {
        if (entity.transform.position.x == transform.position.x && 
            entity.transform.position.y == transform.position.y)
        {
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].binPosition, this, true);
        }
        else
        {
            outputs[0].bin = entity;
            UpdateBins();
        }
    }
}
