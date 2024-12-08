using Godot;
using System;

public partial class Character : CharacterBody2D
{
	public const float Speed = 270.0f;
	public const float JumpVelocity = -440.0f;
	private const double RotationSpeed = Math.PI * 1.5;

	private CpuParticles2D smokeParticles;
	private Sprite2D icon;
	private AnimatedSprite2D eyes;

	public override void _Ready()
	{
		smokeParticles = GetNode<CpuParticles2D>("%SmokeParticles");
		icon = GetNode<Sprite2D>("%Icon");
		eyes = GetNode<AnimatedSprite2D>("%Eyes");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;


		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionPressed("ui_up") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Rotation += direction.X * (float)RotationSpeed * (float)delta;

		Velocity = velocity;
		MoveAndSlide();
	}

	public void PerformDeathSprite()
	{
		icon.Hide();
		eyes.Hide();
		smokeParticles.Emitting = true;
	}

	public void ResetSprite()
	{
		Vector2I frameCoords = icon.FrameCoords;
		icon.FrameCoords = new Vector2I(frameCoords.X, 0); //* Resetting color
		
		icon.Show();
		eyes.Show();
	}
}
