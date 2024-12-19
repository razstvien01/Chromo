using Godot;
using System;

public partial class MiniTrivia : Control
{
    [Signal]
    public delegate void MiniTriviaEndEventHandler();

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    }

    public void PlayAnimation(string animationName) {
        animationPlayer.CurrentAnimation = animationName;
        animationPlayer.Play();
    }

    private void onAnimationFinished(string animationName) {
        EmitSignal(SignalName.MiniTriviaEnd, animationName);
    }
}
