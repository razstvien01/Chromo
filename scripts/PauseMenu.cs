using Godot;

public partial class PauseMenu : Control
{
	[Signal]
	public delegate void UnpauseEventHandler();

	public override void _Ready()
	{
		Button returnBtn = GetNode<Button>("%Return");
		returnBtn.Pressed += () => 
		{
			EmitSignal(SignalName.Unpause);
			Hide();
		};

		Button returnToMainMenuBtn = GetNode<Button>("%ReturnToMainMenu");
		returnToMainMenuBtn.Pressed += () =>
		{
			var menuScene = (PackedScene)GD.Load("res://scenes/Menu.tscn");
			GetTree().ChangeSceneToPacked(menuScene);
		};
	}
}
