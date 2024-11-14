using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] float Speed = 10;
    [Export] Node3D Model;
    [Export] float SmoothingFactor = 0.1f; // For velocity
    [Export] float WalkSmoothingFactor = 0.1f; // For walk value
    [Export] float RotationSmoothingFactor = 0.1f; // For rotation

    [Signal] public delegate void InventoryUpdatedEventHandler();
	public List<MovableItem> Inventory = new();

    private Vector2 targetVelocity = Vector2.Zero;
    private float targetWalkVal = 0.0f;
    private float walkVal = 0.0f;
    private float targetRotation = 0.0f;
    private float rotation = 0.0f;
    private AnimationTree animationTree;

    public override void _Ready()
    {
        animationTree = Model.GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputVec = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        Vector2 movementVec;

        if (inputVec.X != 0 && inputVec.Y != 0)
        {
            movementVec = new Vector2(
                inputVec.X,
                inputVec.Y / 2
            );
        }
        else
        {
            movementVec = inputVec;
        }

        if (movementVec != Vector2.Zero)
        {
            targetRotation = -movementVec.Angle() + Mathf.Pi / 4;
            targetWalkVal = 1.0f;
        }
        else
        {
            targetWalkVal = 0f;
        }

        walkVal = Mathf.Lerp(walkVal, targetWalkVal, WalkSmoothingFactor);
        rotation = Mathf.LerpAngle(rotation, targetRotation, RotationSmoothingFactor);
        Model.Rotation = new Vector3(0, rotation, 0);

        targetVelocity = movementVec.Normalized() * Speed;
        Velocity = Velocity.Lerp(targetVelocity, SmoothingFactor);

        var collision = MoveAndCollide(Velocity * (float)delta);

        if (collision != null)
        {
            Vector2 collisionNormal = collision.GetNormal();

            float dotProduct = Velocity.Dot(collisionNormal);

            if (Mathf.Abs(dotProduct) > 0.9f)
            {
                Velocity -= collisionNormal * dotProduct;
            }
            else
            {
                Velocity -= collisionNormal * dotProduct * 0.5f;
            }
        }

        animationTree.Set("parameters/Blend2/blend_amount", walkVal);

        GetNearestItem();
    }

    public override void _Input(InputEvent ev)
    {
        if (ev.IsActionPressed("interact"))
        {
            Interact();
        }
        else if (ev.IsActionPressed("quit"))
        {
            GetTree().Quit();
        }
    }

    Item? GetNearestItem()
    {
        float closestDistance = int.MaxValue;
        Item closestItem = null;


		foreach (var body in GetNode<Area2D>("InteractionArea").GetOverlappingBodies())
		{
			if (body is Item item) // body is Item
			{
				var distance = Mathf.Abs((item.GlobalPosition - GlobalPosition).LengthSquared());
				if (distance < closestDistance)
				{
					// highlight new closest item
					if (closestItem != null)
					{
						closestItem.Highlighted = false;
					}
					item.Highlighted = true;
					closestDistance = distance;
					closestItem = item;
				}
				else
				{
					item.Highlighted = false;
				}
			}
		}

        return closestItem;
    }


	void ItemExit(Node2D body)
	{
		if (body is Item item)
		{
			item.Highlighted = false;
		}
	}

	void Interact()
	{
		var item = GetNearestItem();

		if (Inventory.Count > 0)
		{
			DropItem();
			return;
		}

		if (item is MovableItem movable)
		{
			// Only hold one item
			Inventory.Clear();
			Inventory.Add(movable);
			movable.PickUp();
			
			EmitSignal("InventoryUpdated");
		}
		else if (item is InteractiveItem interactive)
		{
			interactive.Activate();
		}
	}

    void DropItem()
    {
        if (Inventory.Count == 0) return;

        var item = Inventory[0];
        Inventory.RemoveAt(0);

        float normalizedRotation = -Mathf.Wrap(targetRotation, 0, 2 * Mathf.Pi) + Mathf.Pi / 4;

        Vector2 dropDirection = new Vector2(Mathf.Cos(normalizedRotation), Mathf.Sin(normalizedRotation)).Normalized();
        Vector2 dropPosition = GlobalPosition + dropDirection * 25;

        item.GlobalPosition = dropPosition;
        item.Drop();
        EmitSignal("InventoryUpdated");
    }
}
