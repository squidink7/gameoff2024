using Godot;
using System;

public partial class Lightmask : Node2D
{
	[Export] Texture2D Texture;

	Sprite2D Sprite;
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await ToSignal(GetTree(), "process_frame");
		
		Sprite = new Sprite2D
		{
			Texture = Texture
		};

		Sprite.Scale = Scale;

		ItemsLayer.LightmaskRoot.AddChild(Sprite);
	}

	public override void _Process(double delta)
	{
		Sprite.GlobalPosition = GlobalPosition;
	}
}
