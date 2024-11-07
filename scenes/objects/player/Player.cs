using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] float Speed = 10;
	[Export] Node3D Model;
	[Export] float SmoothingFactor = 0.1f; // Adjust this value for more or less smoothing

	private Vector2 targetVelocity = Vector2.Zero;

	public override void _PhysicsProcess(double delta)
	{
		var inputVec = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		Vector2 movementVec = Vector2.Zero;

		// Check for diagonal input
		if (inputVec.X != 0 && inputVec.Y != 0)
		{
			// Apply isometric transformation for diagonal movement
			movementVec = new Vector2(
				inputVec.X, // Skew the x-axis
				inputVec.Y / 2 // Skew the y-axis
			);
		}
		else
		{
			// Use screen-relative movement for non-diagonal input
			movementVec = inputVec;
		}

		// Walking
		if (movementVec != Vector2.Zero)
		{
			// Rotate towards movement direction
			Model.Rotation = Vector3.Up * (-movementVec.Angle() + Mathf.Pi / 4);

			// Play animation
			Model.GetNode<AnimationPlayer>("AnimationPlayer").Play("BriskWalk");
		}
		else
		{
			Model.GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
		}

		// Calculate target velocity
		targetVelocity = movementVec.Normalized() * Speed;

		// Smoothly interpolate current velocity towards target velocity
		Velocity = Velocity.Lerp(targetVelocity, SmoothingFactor);

		MoveAndSlide();
	}
}
