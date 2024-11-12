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
	}

	public override void _Process(double delta)
	{
		GetNode<Camera2D>("LightmaskViewport/Camera").Transform = Camera.Transform;
	}
}
