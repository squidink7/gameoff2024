using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] float Speed = 10;
	
	public override void _PhysicsProcess(double delta)
	{
		var inputVec = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		Velocity = inputVec * Speed;

		MoveAndSlide();
	}
}
