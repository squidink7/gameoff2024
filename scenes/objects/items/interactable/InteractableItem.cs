using System;
using Godot;

public partial class InteractableItem : Item
{
	[Signal] public delegate void ActivatedEventHandler();
	
	public void Activate()
	{
		GD.Print(Name + " activated");
		EmitSignal("Activated");
	}
}