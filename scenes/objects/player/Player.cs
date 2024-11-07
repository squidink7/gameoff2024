using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] float Speed = 10;
	[Export] Node3D Model;
	
	public override void _PhysicsProcess(double delta)
	{
		var inputVec = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		// Walking
		if (inputVec != Vector2.Zero)
		{
			// Rotate towards movement direction
			Model.Rotation = Vector3.Up * (-inputVec.Angle() + Mathf.Pi/4);

			// Play animation
			Model.GetNode<AnimationPlayer>("AnimationPlayer").Play("Walk");
		}
		else
		{
			Model.GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
		}

		Velocity = inputVec * Speed;

		MoveAndSlide();
	}
}
