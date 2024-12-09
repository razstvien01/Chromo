using Godot;

public partial class TriviaResource : Resource
{
    [Export(PropertyHint.MultilineText)]
    public string Trivia { get; set; }
    [Export]
    public string Title { get; set; }
    [Export]
    public Texture2D Image { get; set; }
    [Export]
    public AudioStream Narration { get; set; }
    [Export]
    public float TriviaAnimationSpeed { get; set; }
}