using Godot;
using System;

public partial class ButtonArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _OnButtonBodyEntered(Node2D body){
		GD.Print("Body Entered.");
		if(body is CharacterBody2D character){
			Sprite2D icon = character.GetNode<Sprite2D>("Icon");
			
			if(icon != null){
				Vector2 frameCoords = icon.FrameCoords;
        GD.Print($"Icon Animation Frame Coords: X = {frameCoords.X}, Y = {frameCoords.Y}");
			}
			else
      {
        GD.Print("Character does not have an Icon node with AnimatedSprite2D.");
      }
		}
	}
}
