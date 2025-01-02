using Godot;
using System;

public partial class GlobalManager : Node
{
	
	public static GlobalManager Instance;
	[Signal]
	public delegate void finish_dEventHandler();
	[Signal]
	public delegate void enemy_boomEventHandler();
	[Signal]
	public delegate void skip_dEventHandler();
	[Signal]
	public  delegate void drive_tankEventHandler();
	[Signal]
	public  delegate void del_tankEventHandler();
	[Signal]
	public  delegate void del_tEventHandler();
	[Signal]
	public  delegate void failEventHandler();
	Timer t;
	public int patron;
	
	
	public override void _Ready()
	{
		Instance = this;
		
	}
	public void shoot (Vector2 tank_pos, Vector2 marker_pos, Node i, bool particl, bool fallow_m, float angle_pushka)
	{
		var _bullet = (PackedScene)ResourceLoader.Load("res://scene/bullet.tscn");
		var bullet = _bullet.Instantiate<CharacterBody2D>();
		bullet.GlobalPosition = marker_pos;
		var b = bullet as Bullet;
		b.player_pos = tank_pos;
		b.fallow_m = fallow_m;
		b.angle_pushka = angle_pushka;
		i.GetParent().AddChild(bullet);
		b.p.Emitting = particl;
		

	}
	public void spawn_d (Vector2 pos, Vector2 sc)
	{
		var _dialog = (PackedScene)ResourceLoader.Load("res://scene/dialog.tscn");
		var dialog = _dialog.Instantiate<CharacterBody2D>();
		dialog.GlobalPosition = pos;
		dialog.Scale = sc;
	}
	
	
	

    
	
}