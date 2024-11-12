using Godot;
using System;

public partial class Item : StaticBody2D
{
	[Export] int LineThickness = 10;
	public bool Highlighted = false;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Highlighted)
		{
			(GetNode<Sprite2D>("Sprite").Material as ShaderMaterial).SetShaderParameter("width", 10);
		}
		else
		{
			(GetNode<Sprite2D>("Sprite").Material as ShaderMaterial).SetShaderParameter("width", 0);
		}
	}

	public void Highlight()
	{
		GD.Print(Name);
	}

	public void Disable()
	{
		Visible = false;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = true;
	}

	public void Enable()
	{
		Visible = true;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = false;
	}

	public Texture2D GetTexture()
	{
		return GetNode<Sprite2D>("Sprite").Texture;
	}
}
