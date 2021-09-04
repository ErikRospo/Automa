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

    private void Start()
    {
        transform.localScale = new Vector2(sizeAdjust, sizeAdjust);

        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();

        animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    // Togles enabling a corner conveyor
    public void ToggleCorner(bool rotateUp)
    {
        if (rotateUp) outputs[0].position = new Vector2(0, outputs[0].position.x);
        else outputs[0].position = new Vector2(0, -outputs[0].position.x);

        isCorner = true;
        animator.enabled = !animator.enabled;
        if (!animator.enabled)
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buildings/ConveyorTurn");
    }

    // Sets a conveyor bin
    public void SetBin(Entity entity)
    {
        if (entity.transform.position == inputs[0].position)
        {
            inputs[0].bin = entity;
            acceptingEntities = false;
        }
        else outputs[0].bin = entity;
        UpdateBins();
    }

    public override void UpdateBins()
    {
        // Checks the front container
        if (outputs[0].bin != null && outputs[0].target != null && outputs[0].target.acceptingEntities)
        {
            if (outputs[0].target.PassEntity(outputs[0].bin))
            {
                outputs[0].bin = null;
                outputs[0].reserved = false;
            }
        }
        
        // Check the back container
        if (inputs[0].bin != null && outputs[0].bin == null && !outputs[0].reserved)
        {
            if (isCorner) inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this);
            else inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].position, this);

            inputs[0].bin = null;
            acceptingEntities = true;
            outputs[0].reserved = true;
            inputs[0].reserved = false;

            if (inputs[0].target != null)
                inputs[0].target.UpdateBins();
        }
    }

    public void CornerCheck(Building conveyor)
    {
        // Check to make sure conveyor is not facing the same direction
        if (conveyor.rotation == rotationType.NORTH && rotation == rotationType.WEST ||
            conveyor.rotation == rotationType.EAST && rotation == rotationType.NORTH ||
            conveyor.rotation == rotationType.SOUTH && rotation == rotationType.EAST ||
            conveyor.rotation == rotationType.WEST && rotation == rotationType.SOUTH)
        {
            conveyor.outputs[0].target = this;
            conveyor.UpdateBins();
            inputs[0].target = conveyor;
        }
    }

    public override bool PassEntity(Entity entity)
    {
        acceptingEntities = false;
        inputs[0].reserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].position, this);
        return true;
    }

    public override void ReceiveEntity(Entity entity)
    {
        if (entity.transform.position == transform.position)
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].position, this);
        else SetBin(entity);
    }
}
