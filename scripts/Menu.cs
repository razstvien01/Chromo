using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}	
	
	public void _OnStartButtonPressed(){
		GD.Print("Pressed Start Button");
	}
	// _on_option_button_pressed
	public void _OnOptionButtonPressed(){
		GD.Print("Pressed Option");
	}
}
