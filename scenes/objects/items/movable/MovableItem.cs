using System;
using Godot;

public partial class MovableItem : Item
{
	public void PickUp()
	{
		Visible = false;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = true;
	}

	public void Drop()
	{
		Visible = true;
		GetNode<CollisionShape2D>("CollisionShape").Disabled = false;
	}

	public Texture2D GetTexture()
	{
		return GetNode<Sprite2D>("Sprite").Texture;
	}
}