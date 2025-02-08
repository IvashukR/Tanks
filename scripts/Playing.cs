using Godot;
using System;

public partial class Playing : State
{
	[Export] public float speed = 100.0f;
	private Vector2 dir;
	[Export] private CharacterBody2D voin = new CharacterBody2D();
	private Node fsm;
	public Voin v;
	private Marker2D marker;
	private bool can_shoot = true;
	private Timer timer;
	private Node _parent;
	private bool pressed_on_ai = false;
	//private BoxContainer info;
	//private Label patron_l;
	//private Label hp_l;
	[Export] private string name_unit = "Voin";
	private AnimationPlayer anim;
	public override void _Ready()
	{
		//patron_l = GetNode<Label>("%patron_l");
		//hp_l = GetNode<Label>("%hp_l");
		//info = GetNode<BoxContainer>("%info");
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
		v.Velocity = dir  * v.speed * 100.0f * (float)delta;
		v.MoveAndSlide();
	}
	public override void Inp(InputEvent @event)
    {
		if(GlobalManager.Instance.block_input)return;
		if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot && v.patron_count >= 0)
            {
				if(_parent is Level1 level)
                {
					GD.Print(",CHEK");
                    foreach(TextureButton btn in level.all_btn_ui)
                    {
						var buttonRect = btn.GlobalPosition;
    					GD.Print($"Button {btn.Name}: Rect: {buttonRect}, Mouse Position: {mouseEvent.GlobalPosition}");
                        if(btn.GetGlobalRect().HasPoint(mouseEvent.GlobalPosition) && btn.Visible == true)
						{
							GD.Print("CHEK");
							return;
						}
                    }
                }
                GlobalManager.Instance.shoot(marker.GlobalPosition, marker.GlobalPosition, this, true, true, marker.Rotation, new Vector2(0.1f, 0.1f), v.damage);
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
