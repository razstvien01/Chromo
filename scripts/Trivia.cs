using Godot;
using System;
using System.Collections.Generic;

public partial class Trivia : Control
{
    private TriviaResource _triviaResource;

    private readonly Dictionary<int, string[]> triviaImages = new()
    {
        { 1, new[] { "res://assets/Trivias/chromosomes.png", "res://assets/Trivias/23pairs.png" } },
    };

    private Label triviaText;
    private Label triviaTitle;
    private TextureRect triviaImage;
    private AudioStreamPlayer triviaNarration;
    private Button proceedButton;
    private AnimationPlayer triviaAnimation;

    public TriviaResource TriviaResource
    {
        get => _triviaResource;
        set
        {
            _triviaResource = value;
            if (_triviaResource == null) return;

            UpdateTriviaContent();
        }
    }

    public override void _Ready()
    {
        InitializeNodes();
    }

    private void InitializeNodes()
    {
        triviaText = GetNode<Label>("%Trivia");
        triviaTitle = GetNode<Label>("%Title");
        triviaImage = GetNode<TextureRect>("%Image");
        triviaNarration = GetNode<AudioStreamPlayer>("%Narration");
        proceedButton = GetNode<Button>("ProceedButton");
        triviaAnimation = GetNode<AnimationPlayer>("TriviaAnimation");
    }

    private void UpdateTriviaContent()
    {
        triviaText.Text = $"{_triviaResource.Title}{_triviaResource.Trivia}";
        triviaTitle.Text = _triviaResource.Title;
        GetNode<Label>("PanelContainer/TriviaScroll/MarginContainer/VBoxContainer/Trivia").Text = _triviaResource.Trivia;
        triviaNarration.Stream = _triviaResource.Narration;
        triviaAnimation.SpeedScale = _triviaResource.TriviaAnimationSpeed;
        triviaNarration.Play();
    }

    public void _OnProceedButtonPressed()
    {
        GD.Print("Proceed Button Pressed.");
        QueueFree();
    }

    private void _OnNarrationFinished()
    {
        proceedButton.Visible = true;
				GetNode<Panel>("Panel").Visible = false;
        SetTriviaPanelVisibility(true);
        AddTriviaImages(1);
    }

    private void SetTriviaPanelVisibility(bool isVisible)
    {
        GetNode<PanelContainer>("PanelContainer").Visible = isVisible;
        triviaTitle.Visible = isVisible;
    }

    private void AddTriviaImages(int key)
    {
        VBoxContainer vbox = GetNode<VBoxContainer>("PanelContainer/TriviaScroll/MarginContainer/VBoxContainer");

        if (!triviaImages.TryGetValue(key, out string[] imagePaths))
        {
            GD.PrintErr($"Key {key} does not exist in triviaImages dictionary.");
            return;
        }

        foreach (string imagePath in imagePaths)
        {
            AddTextureRectToVBox(vbox, imagePath);
        }
    }

    private void AddTextureRectToVBox(VBoxContainer vbox, string imagePath)
    {
        Texture2D texture = GD.Load<Texture2D>(imagePath);
        if (texture == null)
        {
            GD.PrintErr($"Failed to load texture at path: {imagePath}");
            return;
        }

        TextureRect textureRect = CreateTextureRect(texture);
        vbox.AddChild(textureRect);
    }

    private TextureRect CreateTextureRect(Texture2D texture)
    {
        return new TextureRect
        {
            Texture = texture,
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
        };
    }
}
