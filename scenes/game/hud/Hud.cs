using Godot;
using System;

public partial class Hud : CanvasLayer
{
	Player Player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetParent<Player>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void UpdateInventory()
	{
		var inventory = Player.Inventory;
		GetNode<TextureRect>("Inventory/Item").Texture = null;

		foreach (var item in inventory)
		{
			var texture = item.GetTexture();
			GetNode<TextureRect>("Inventory/Item").Texture = texture;
		}
	}
}
