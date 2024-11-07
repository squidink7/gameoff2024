using Godot;
using System;

public partial class ItemsLayer : CanvasGroup
{
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
		(Material as ShaderMaterial).SetShaderParameter("occluder_texture", player.GetLightmaskViewport().GetTexture());
	}
}
