using Godot;

public partial class MiniTriviaArea : Area2D
{
	[Export]
	public string AnimationName { get; set; }
	
    [Signal]
    public delegate void MiniTriviaStartedEventHandler(string animationName);

    private bool isDisabled = false;

    public void Enable() {
        isDisabled = false;
        Show();
    }

    public void Disable() {
        isDisabled = true;
        Hide();
    }

    private void _on_body_entered(Node2D body) {
        if(!isDisabled && body is Character) {
            EmitSignal(SignalName.MiniTriviaStarted, AnimationName);
        }
    }

}
