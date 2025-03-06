using Godot;
using System;

public partial class Playing : State
{
	private Vector2 dir;
	[Export] private CharacterBody2D voin = new CharacterBody2D();
	private Node fsm;
	public Voin v;
	private Marker2D marker;
	private bool can_shoot = true;
	private Timer timer;
	private Node _parent;
	private bool pressed_on_ai = false;
	private AnimationPlayer anim;
	private Label patron_l;
	private BoxContainer info;
	public override void _Ready()
	{
		info = GetNode<BoxContainer>("%info");
		patron_l = GetNode<Label>("%patron_l");
		anim = GetNode<AnimationPlayer>("%anim");
		fsm = GetNode<Node>("%FSM");
		var parent = fsm.GetParent();
    	if (parent is Voin)
    	{
        	v = (Voin)parent;
    	}
		_parent = v.GetParent();
		timer = new Timer();
		timer.WaitTime = v.perezaryad;
		timer.OneShot = true;
		AddChild(timer);
		marker = GetNode<Marker2D>("%marker");
		timer.Timeout += () => can_shoot = true;
		patron_l.Text = $"{v.name_unit} Patron: {v.patron_count}";
		v.TreeExited += () => v = null;
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
		v.Velocity = dir  * v.speed * (float)delta;
		v.MoveAndSlide();
	}
	public override void Inp(InputEvent @event)
    {
		if(GlobalManager.Instance.block_input)return;
		if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot && v.patron_count > 0)
            {
				if(_parent is Level1 level)
                {
                    foreach(TextureButton btn in level.all_btn_ui)
                    {
                        if(btn.GetGlobalRect().HasPoint(mouseEvent.GlobalPosition) && btn.Visible)
						{
							return;
						}
                    }
                }
                GamaUtilits.shoot(marker.GlobalPosition, marker.GlobalPosition, this, true,marker.Rotation, new Vector2(0.1f, 0.1f), v.damage, -1, 500);
				can_shoot = false;
				v.patron_count--;
				patron_l.Text = $"{v.name_unit} Patron: {v.patron_count}";
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
