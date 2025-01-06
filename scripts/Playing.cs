using Godot;
using System;

public partial class Playing : State
{
	[Export] public float speed = 100.0f;
	private Vector2 dir;
	[Export] private CharacterBody2D voin = new CharacterBody2D();
	private Sprite2D gun;
	private Node fsm;
	public Voin v;
	private Marker2D marker;
	private bool can_shoot = true;
	private Timer timer;
	public override void _Ready()
	{
		fsm = GetNode<Node>("%FSM");
		var parent = fsm.GetParent();
    	if (parent is Voin)
    	{
        	v = (Voin)parent;
    	}
		timer = new Timer();
		timer.WaitTime = v.perezaryad;
		timer.OneShot = true;
		AddChild(timer);
		gun = GetNode<Sprite2D>("%gun");
		marker = GetNode<Marker2D>("%marker");
		timer.Timeout += () => can_shoot = true;
	}
	
	public override void Process(double delta)
	{
		float ang = (GetViewport().GetMousePosition() - gun.GlobalPosition).Angle();
		gun.Rotation = ang;
	}
	public override void PhysicsProcess(double delta)
	{
		dir = Input.GetVector("l", "r", "d", "u").Normalized();
		v.Velocity = dir  * v.speed;
		v.MoveAndSlide();
	}
	public override void Inp(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
            
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot && v.patron_count >= 0)
            {
                GlobalManager.Instance.shoot(gun.GlobalPosition, marker.GlobalPosition, this, true, true, 0.0f, new Vector2(0.1f, 0.1f), v.damage);
				can_shoot = false;
				v.patron_count--;
				timer.Start();
            }
        }
        
    }
	public override void _ExitTree()
	{
    if (v != null)
    {
        v.QueueFree();
    }
	}


}
