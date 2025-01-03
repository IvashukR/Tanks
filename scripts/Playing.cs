using Godot;
using System;

public partial class Playing : State
{
	[Export] public float speed = 100.0f;
	private Vector2 dir;
	[Export] private CharacterBody2D Voin = new CharacterBody2D();
	private Sprite2D gun;
	private Voin voin;
	private Marker2D marker;
	private bool can_shoot = true;
	private Timer timer;
	public override void _Ready()
	{
		voin = (Voin)Voin;
		timer = new Timer();
		timer.WaitTime = voin.perezaryad;
		timer.OneShot = true;
		AddChild(timer);
		gun = GetNode<Sprite2D>("gun");
		marker = GetNode<Marker2D>("%marker");
		timer.Timeout += () => can_shoot = true;
	}
	
	public override void Process(double delta)
	{
		float ang = (GetViewport().GetMousePosition() - gun.GlobalPosition).Angle();
		gun.Rotation += ang;
	}
	public override void PhysicsProcess(double delta)
	{
		dir = Input.GetVector("l", "r", "d", "u").Normalized();
		Voin.Velocity = dir  * voin.speed;
		Voin.MoveAndSlide();
	}
	public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
            
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot)
            {
                GlobalManager.Instance.shoot(gun.GlobalPosition, marker.GlobalPosition, this, true, true, 0.0f, new Vector2(0.1f, 0.1f));
				can_shoot = false;
				timer.Start();
            }
        }
        
    }

}
