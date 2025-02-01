using Godot;
using System;


public partial class TankRedMenu : StaticBody2D
{
	public float angle = 0;
	
	private bool emit = false;
	public int  radius = 100;
	private Vector2 center;
	private bool isMouse = false;
	[Export] public double radialVel = 0;
	public Node2D target_pos;
	public StaticBody2D tankBlue;
	public Timer timer;
	private CpuParticles2D particles;
	private RandomNumberGenerator rng;
	private CpuParticles2D particles2;
	private Sprite2D _break;
	private Marker2D marker;
	private Sprite2D red_t;
	private CollisionShape2D col;
	private Timer t;
	private bool can_shoot = true;
	private TextureButton start;
	private bool move_r = true ;
	public override void _Ready()
	{
		Node p = GetNode("%Phone");

		Phone phone = (Phone)p;
		start = GetNode<TextureButton>("%btn");
		start.Pressed += async () => 
		{
			phone.Show();
			phone.anim_phone.Play("close");
			await ToSignal(phone.anim_phone, "animation_finished");
			GetTree().ChangeSceneToFile("res://scene/trenirovka.tscn");
		};
		marker = GetNode<Marker2D>("%marker");
		red_t = GetNode<Sprite2D>("%red_t");
		col = GetNode<CollisionShape2D>("%col");
		center = this.GlobalPosition;
		target_pos = GetNode<Node2D>("%target_pos");
		tankBlue = GetNode<StaticBody2D>("%TankedBlue");
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimeout;
		timer.Start(0.5f);
		rng = new RandomNumberGenerator();
		particles = GetNode<CpuParticles2D>("%particles");
		_break = GetNode<Sprite2D>("%break");
		particles2 = GetNode<CpuParticles2D>("%p2");
		t = GetNode<Timer>("%t");
		t.Timeout += () => particles2.QueueFree();
		t.WaitTime = 2.5f;
		GlobalManager.Instance.del_tank += () => {
		particles2.Emitting = true;
		red_t.Visible = false;
		col.CallDeferred("set_disabled", true);
		_break.Visible = true;
		move_r = false;
		t.Start();
		};
	}
	private void OnTimeout()
	{
		if (!isMouse)
		{
			tankBlue.Rotation += rng.RandfRange(0.1f, 0.2f);
			timer.Start(rng.RandfRange(0.1f, 0.5f));
		}
		
	}

	public override void _Process(double delta)

	{
		
		if (move_r)
		{
			angle += 0.1f;
			Rotation += (float) radialVel *(float) delta;
		}
		if (angle > MathF.Tau)
		{
			angle -= MathF.Tau;
		}
		float x = center.X + radius * MathF.Cos(angle);
		float y = center.Y + radius * MathF.Sin(angle);
		GlobalPosition = new Vector2(x , y);
		Vector2 direction = target_pos.GlobalPosition - tankBlue.GlobalPosition;
		if (direction.Length()> 0.1f)
		{
			tankBlue.Position += direction.Normalized() * 100  * (float)delta ;
		}
		else
		{
			isMouse = true;

		}
		if (isMouse )
		{
			timer.Stop();
			tankBlue.LookAt(GetGlobalMousePosition());
			particles.Emitting = false;
			

		}
		if (isMouse && !emit)
		{
			GlobalManager.Instance.EmitSignal("drive_tank");
			emit = true;
		}
		
		
		
		
		
	}
    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
            
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && isMouse && can_shoot)
            {
                GlobalManager.Instance.shoot(tankBlue.GlobalPosition, marker.GlobalPosition, this, false, false, 0.0f, new Vector2(0.165f, 0.171f), 75);
				can_shoot = false;
            }
        }
        
    }

}
