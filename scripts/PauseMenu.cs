using Godot;

public partial class PauseMenu : Control
{
	public override void _Ready()
	{
		Button returnBtn = GetNode<Button>("%Return");
		returnBtn.Pressed += () => 
		{
			Hide();
		};

		Button returnToMainMenuBtn = GetNode<Button>("%ReturnToMainMenu");
		returnToMainMenuBtn.Pressed += () =>
		{
			var menuScene = (PackedScene)GD.Load("res://scenes/Menu.tscn");
			GetTree().ChangeSceneToPacked(menuScene);
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_cancel"))
		{
			Show();
		}
	}
}
