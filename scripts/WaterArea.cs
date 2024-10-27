using Godot;
using System;

public partial class WaterArea : Area2D
{
	[Export]
	public WaterType waterType{ get; set; }
	
	public enum WaterType
	{
		PURPLE, YELLOW
	}

	public override void _Ready()
	{

	}
	
	public override void _Process(double delta)
	{

	}
	
	public void _OnBodyEntered(Node2D body){
		
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
