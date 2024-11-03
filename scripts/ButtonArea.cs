using Godot;
using System;
using System.Collections;
using System.Numerics;

public partial class ButtonArea : Area2D
{
	[Export]
	public ButtonType buttonType { get; set; }

	private AudioStreamPlayer sfxPlayer;

	public enum ButtonType
	{
		YELLOW, PURPLE
	}

	public override void _Ready()
	{
		sfxPlayer = GetNode<AudioStreamPlayer>("%SfxPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnButtonBodyEntered(Node2D body)
	{
		if (body is CharacterBody2D character)
		{
			sfxPlayer.Play();

			Sprite2D icon = character.GetNode<Sprite2D>("Icon");

			if (icon != null)
			{
				int targetFrame = 0;
				
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
