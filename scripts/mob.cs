using Godot;
using System;

public partial class mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// deletes mob when walks off screen
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
