using Godot;

public partial class TriviaResource : Resource
{
    [Export]
    public int TriviaLevel { get; set; }
    
    [Export(PropertyHint.MultilineText)]
    public string Trivia { get; set; }
    [Export]
    public string Title { get; set; }
    [Export]
    public AudioStream Narration { get; set; }
    [Export]
    public float TriviaAnimationSpeed { get; set; }
}