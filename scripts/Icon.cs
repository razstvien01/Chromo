using Godot;
using System;
using System.Diagnostics;

public partial class Icon : Sprite2D
{
	private float SPEED = 200.0f;
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 position = Position;

		if (Input.IsActionPressed("ui_right"))
		{
			position.X += SPEED * (float)delta;
			RotationDegrees += 1;
		}

		if (Input.IsActionPressed("ui_left"))
		{
			position.X -= SPEED * (float)delta;
			RotationDegrees += -1;
		}
		Position = position;
	}
	
	private void __OnRightPressed(){
		GD.Print("Right Button Pressed");
	}
	
	private void __OnLeftPressed(){
		GD.Print("Left Button Pressed");
	}
}
