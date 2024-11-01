using Godot;
using System;

public partial class WaterArea : Area2D
{
	[Export]
	public WaterType waterType { get; set; }
	[Signal]
	public delegate void ShowGameOverEventHandler();

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

	public void _OnBodyEntered(Node2D body)
	{

		if (body is CharacterBody2D character)
		{
			Sprite2D icon = character.GetNodeOrNull<Sprite2D>("Icon");
			if (icon == null)
			{
				GD.Print("Character does not have an 'Icon' node with a Sprite2D.");
				return;
			}

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

			if (targetY != -1 && frameCoords.Y != targetY)
			{
				icon.FrameCoords = new Vector2I(frameCoords.X, 0); //* Resetting color

				Node2D uiControls = GetParent().GetNode<Node2D>("Controller");
				uiControls.Visible = false;

				// character.Position = new Vector2(60, 60);	//* Back to start position
				EmitSignal(SignalName.ShowGameOver);

			}
		}
	}
}
