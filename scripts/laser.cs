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
		// Move bullet in direction
		Position +=  velocity * Speed *(float)delta;

		// Rotate the bullet based on the direction vector
        RotateToPoint(velocity);
	}
	
	 // Rotate the bullet sprite to point towards a specific direction
    private void RotateToPoint(Vector2 direction)
    {
        // Calculate the rotation angle in radians using atan2
        float rotationRadians = Mathf.Atan2(direction.Y, direction.X);

        // Convert the angle to degrees
        float rotationDegrees = Mathf.RadToDeg(rotationRadians);

        // Set the rotation of the bullet sprite
        RotationDegrees = rotationDegrees;
    }
}
