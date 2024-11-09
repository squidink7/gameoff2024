using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] float Speed = 10;
	[Export] Node3D Model;
	[Export] float SmoothingFactor = 0.1f; // For velocity
	[Export] float WalkSmoothingFactor = 0.1f; // For walk value
	[Export] float RotationSmoothingFactor = 0.1f; // For rotation

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
		GetViewport().Connect("size_changed", Callable.From<Vector2I>((size) => { GetLightmaskViewport().Size = size; }));
	}
	
	public override void _PhysicsProcess(double delta)
	{
		var inputVec = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		Vector2 movementVec = Vector2.Zero;

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

		animationTree.Set("parameters/Blend2/blend_amount", walkVal);

		MoveAndSlide();

		GetNearestItem();
	}

	Node2D? GetNearestItem()
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
					closestDistance = distance;
					closestItem = item;
					if (closestItem != null)
					{
						item.Highlighted = true;
					}
					item.Highlighted = true;
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

	public SubViewport GetLightmaskViewport()
	{
		return GetNode<SubViewport>("LightmaskViewport");
	}
}
