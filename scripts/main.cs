using Godot;
using System;

public partial class main : Node
{
	[Export]
    public PackedScene MobScene { get; set; }

    private int _score;
	// stop timers on end game
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<hud>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		var player = GetNode<player>("player");
		player.GetNode<AudioStreamPlayer>("LaserSound").Stop();
		GetNode<AudioStreamPlayer>("Death").Play();
	}
	// set up new game
	public void NewGame()
	{
		_score = 0;

		var player = GetNode<player>("player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		GetNode<AudioStreamPlayer>("Music").Play();
	}
	//increment score every timeout
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<hud>("HUD").UpdateScore(_score);
	}
	// start other timer when start timer has started
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	private void OnMobTimerTimeout()
	{
		// Note: Normally it is best to use explicit types rather than the `var`
		// keyword. However, var is acceptable to use here because the types are
		// obviously Mob and PathFollow2D, since they appear later on the line.

		// Create a new instance of the Mob scene.
		mob mob = MobScene.Instantiate<mob>();

		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("mobPath/mobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mob.Position = mobSpawnLocation.Position;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		// mob.Rotation = direction;

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
