using Godot;
using System;

public partial class laser : Area2D
{
	[Export]
	public int Speed = 750;
	
	private Vector2 velocity = Vector2.Zero;
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("mobs")){
			body.QueueFree();
		}
		QueueFree();
	}
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
	// set intial direction of the bullet
	public void setDirection(Vector2 direction)
	{
		velocity = direction.Normalized();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position +=  velocity * Speed *(float)delta;
	}
	
}
