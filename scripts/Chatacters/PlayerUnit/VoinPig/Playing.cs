using Godot;
using System;

public partial class Playing : State
{
	private Vector2 dir;
	private Node fsm;
	[Export] private UnitLogic unit;
	private Marker2D marker;
	private bool can_shoot = true;
	private Timer timer;
	private bool pressed_on_ai = false;
	private AnimationPlayer anim;
	private Label patron_l;
	private BoxContainer info;
	[Export] private CharacterBody2D v;
	public override void _Ready()
	{
		info = GetNode<BoxContainer>("%info");
		patron_l = GetNode<Label>("%patron_l");
		anim = GetNode<AnimationPlayer>("%anim");
		fsm = GetNode<Node>("%FSM");
		timer = new Timer();
		timer.WaitTime = unit.stats.perezaryad;
		timer.OneShot = true;
		AddChild(timer);
		marker = GetNode<Marker2D>("%marker");
		timer.Timeout += () => can_shoot = true;
		patron_l.Text = $"{unit.name_unit} Patron: {unit.stats.patron_count}";
		unit.TreeExited += () => unit = null;
	}
	public override void Enter()
	{
		info.Show();
	}
	public override void Exit()
	{
		if(info != null)info.Hide();
	}
	public override void Process(double delta)
	{
		
		Vector2 mouse_pos = GetViewport().GetMousePosition();
		v.LookAt(mouse_pos);
		if (v.Velocity.Length() > 0)
		{
			anim.Play("go");
		}
		else anim.Stop();
	}
	public override void PhysicsProcess(double delta)
	{
		dir = Input.GetVector("l", "r", "d", "u").Normalized();
		v.Velocity = dir  * unit.stats.speed * (float)delta;
		v.MoveAndSlide();
	}
	public override void Inp(InputEvent @event)
    {
		if(GlobalManager.Instance.block_input)return;
		if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot && unit.stats.patron_count > 0)
            {
                Shoot();
            }
        }
        
    }
	private void Shoot()
	{
		GamaUtilits.shoot(marker.GlobalPosition, marker.GlobalPosition, this, true,marker.Rotation, new Vector2(0.1f, 0.1f), unit.stats.damage, -1, 500);
		can_shoot = false;
		unit.stats.patron_count--;
		patron_l.Text = $"{unit.name_unit} Patron: {unit.stats.patron_count}";
		timer.Start();
	}

}
