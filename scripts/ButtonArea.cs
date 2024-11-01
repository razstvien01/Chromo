using Godot;
using System;
using System.Collections;
using System.Numerics;

public partial class ButtonArea : Area2D
{
	[Export]
	public ButtonType buttonType { get; set; }
	public enum ButtonType
	{
		YELLOW, PURPLE
	}

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnButtonBodyEntered(Node2D body)
	{
		GD.Print("Body Entered.");
		if (body is CharacterBody2D character)
		{
			Sprite2D icon = character.GetNode<Sprite2D>("Icon");

			if (icon != null)
			{
				int targetFrame = 0;
				
				//* Change color here
				switch (buttonType)
				{
					case ButtonType.YELLOW:{
						targetFrame = 1;
						break;
					}
					case ButtonType.PURPLE:{
						targetFrame = 2;
						break;
					}
				}
				icon.FrameCoords = new Vector2I(icon.FrameCoords.X, targetFrame);
			}
			else
			{
				GD.Print("Character does not have an Icon node with AnimatedSprite2D.");
			}
		}
	}
}
