using System;
using Godot;

public partial class ActivatableItem : StaticBody2D
{
	[Signal] public delegate void ActivatedEventHandler();
	[Export] bool Repeatable = false;

	bool IsActive = false;

	public virtual void Activate()
	{
		if (!Repeatable)
		{
			if (IsActive) return;
			else IsActive = true;
		}
		
		GD.Print(Name + " activated");
		_Activate();
		EmitSignal("Activated");
	}

	public virtual void _Activate() {}
}