using Godot;
using System;
using System.Xml.Resolvers;

public partial class player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	// speed at which player moves
	[Export]
	public int Speed { get; set; } = 400;

	

	private Boolean cooldown = true;
	private Boolean canShoot = false;
	private Boolean playerPointLeft = false;

	public Vector2 ScreenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}
	public void Start(Vector2 position)
	{
		Position = position;
		canShoot = true;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// movement vectors
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
			playerPointLeft = false;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
			playerPointLeft = true;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			// flip if velocity is negative
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		if (Input.IsActionPressed("shoot"))
		{
			Shoot();
		}
	}
	private void OnBodyEntered(Node2D body)
	{
		canShoot = false; 
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	private void Shoot()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var scene = GD.Load<PackedScene>("res://scenes/Laser.tscn"); // Adjust the path as needed
        var muzzle1 = GetNode<Marker2D>("AnimatedSprite2D/Muzzle1");
		var muzzle2 = GetNode<Marker2D>("AnimatedSprite2D/Muzzle2");
        var laser = scene.Instantiate() as laser;
		Vector2 direction;

		// Get the mouse position
        var targetPosition = GetGlobalMousePosition();

        // Calculate the direction vector from the player to the mouse position
        direction = (targetPosition - Position).Normalized();

        if (laser != null && cooldown && canShoot)
        {
            GetTree().CurrentScene.AddChild(laser);
			// change the marker if player points to left or right
			if (playerPointLeft) {
				laser.GlobalPosition = muzzle2.GlobalPosition;
				// do not let player shoot behind them
				if (direction.X > 0 && direction.Y > 0) {
					direction = new Vector2(0,1);
				}
				if (direction.X > 0 && direction.Y < 0) {
					direction = new Vector2(0,-1);
				}
			} else {
				// do not let player shoot behind them
				if (direction.X < 0 && direction.Y > 0) {
					direction = new Vector2(0,1);
				}
				if (direction.X < 0 && direction.Y < 0) {
					direction = new Vector2(0,-1);
				}
				laser.GlobalPosition = muzzle1.GlobalPosition;
			}
			// change direction of lase based on the way the muzzle is pointed
			laser.setDirection(direction);
			cooldown = false;
			GetNode<Timer>("LaserTimer").Start();
			GetNode<AudioStreamPlayer>("LaserSound").Play();
        }
	}
	private void OnLaserTimerTimeout()
	{
		cooldown = true;
		GetNode<Timer>("LaserTimer").Stop();

	}
}
