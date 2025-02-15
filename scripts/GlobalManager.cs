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
	[Signal]
	public  delegate void change_moneyEventHandler();
	[Signal]
	public  delegate void pick_unitEventHandler();
	[Signal]
	public  delegate void destroyed_townEventHandler();
	[Signal]
	public  delegate void destroyed_enemy_townEventHandler();
	[Signal]
	public  delegate void take_damageEventHandler(Node2D node, Bullet bullet);
	[Signal]
	public  delegate void card_clickEventHandler();
	public Node2D temp_pick_unit;		
	Timer t;
	public bool block_input;
	private int money = 200;
	public bool block_drop_unit;
	public int Money
	{
		set
		{
			money = value;
			
		}
		get{return money;}
	}
	
	
	public override void _Ready()
	{
		Instance = this;
		
	}
	
	
	
}
