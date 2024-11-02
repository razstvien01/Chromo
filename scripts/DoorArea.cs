using Godot;
using System;

public partial class DoorArea : Area2D
{
	[Signal]
	public delegate void NextSceneEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _OnDoorAreaBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D character)
		{
			Sprite2D icon = character.GetNode<Sprite2D>("Icon");

			if (icon != null)
			{
				icon.FrameCoords = (icon.FrameCoords.X < 4) ? new Vector2I(icon.FrameCoords.X + 1, icon.FrameCoords.Y) : new Vector2I(icon.FrameCoords.X, icon.FrameCoords.Y);
				
				EmitSignal(SignalName.NextScene);
			}
		}
	}
}
