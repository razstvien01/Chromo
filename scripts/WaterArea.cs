using Godot;
using System;

public partial class WaterArea : Area2D
{
	[Export]
	public WaterType waterType { get; set; }
	[Signal]
	public delegate void ShowGameOverEventHandler();

	private AudioStreamPlayer sfxPlayer;
	private Character currentPlayerEntered;
	private bool isDead = false;
	public enum WaterType
	{
		PURPLE, YELLOW
	}

	public override void _Ready()
	{
		sfxPlayer = GetNode<AudioStreamPlayer>("%SfxPlayer");
	}

	public override void _Process(double delta)
	{
		if (currentPlayerEntered != null)
		{
			if (isDiffColor() && !isDead) 
				_OnBodyEntered(currentPlayerEntered);
		}
	}

	private bool isDiffColor()
	{
		Sprite2D icon = currentPlayerEntered.GetNodeOrNull<Sprite2D>("Icon");
		Vector2I frameCoords = icon.FrameCoords;
		int targetY = -1;

		switch (waterType)
		{
			case WaterType.YELLOW:
				targetY = 1;
				break;

			case WaterType.PURPLE:
				targetY = 2;
				break;
		}
		return frameCoords.Y != targetY;
	}

	private void _OnBodyExited(Node2D body)
	{
		if (body is Character character)
		{
			currentPlayerEntered = null;
			isDead = false;
		}
	}

	public void _OnBodyEntered(Node2D body)
	{
		if (body is Character character)
		{
			currentPlayerEntered = character;
			Sprite2D icon = character.GetNodeOrNull<Sprite2D>("Icon");
			if (icon == null)
			{
				GD.Print("Character does not have an 'Icon' node with a Sprite2D.");
				return;
			}

			if (isDiffColor())
			{
				isDead = true;
				character.PerformDeathSprite();
				sfxPlayer.Play();

				Node2D uiControls = GetParent().GetNode<Node2D>("Controller");
				uiControls.Visible = false;
				
				EmitSignal(SignalName.ShowGameOver);
			}
		}
	}
}
