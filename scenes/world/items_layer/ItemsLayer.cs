using Godot;
using System;

public partial class ItemsLayer : CanvasGroup
{
	Camera2D Camera;

	// Locate and set shader material
	public override void _Ready()
	{
		// return;
		var players = GetParent().FindChildren("Player", "");

		if (players.Count == 0)
		{
			GD.Print("No Player in scene, All Items visible");
			return;
		}

		var player = players[0] as Player;
		
		Camera = player.GetNode<Camera2D>("WorldCamera");

		// Render everything in the game world
		GetNode<SubViewport>("LightmaskViewport").World2D = GetViewport().World2D;
		// Set main viewport to not show lightmasks
		GetTree().Root.GetViewport().SetCanvasCullMaskBit(1, false);
	}

	public override void _Process(double delta)
	{
		GetNode<Camera2D>("LightmaskViewport/Camera").GlobalPosition = Camera.GlobalPosition;
	}
}
