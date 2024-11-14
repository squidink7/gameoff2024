using Godot;
using System;

public partial class ItemsLayer : CanvasGroup
{
	public static Node LightmaskRoot;

	Camera2D Camera;

	public override void _Ready()
	{
		// Root node for all lightmasks to parent themselves to
		LightmaskRoot = GetNode("LightmaskViewport");
		
		// Find Player in scene and get reference to their camera
		var players = GetParent().FindChildren("Player", "");

		if (players.Count == 0)
		{
			GD.Print("No Player in scene, All Items visible");
			return;
		}

		var player = players[0] as Player;
		
		Camera = player.GetNode<Camera2D>("WorldCamera");

		// Set material
		(Material as ShaderMaterial).SetShaderParameter("occluder_texture", (LightmaskRoot as SubViewport).GetTexture());
	}

	public override void _Process(double delta)
	{
		// Follow Player's camera
		if (Camera == null) return;
		GetNode<Camera2D>("LightmaskViewport/Camera").GlobalPosition = Camera.GlobalPosition;
	}
}
