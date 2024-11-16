using Godot;
using System;

public partial class SandstoneDoor : ActivatableItem
{
	public override void _Activate()
	{
		GetNode<AnimationPlayer>("Animation").Play("open");
	}
}
