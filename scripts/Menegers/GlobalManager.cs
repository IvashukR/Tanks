using Godot;
using System;
using System.Collections.Generic;
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
	public  delegate void del_tankEventHandler();
	[Signal]
	public  delegate void del_tEventHandler();
	[Signal]
	public  delegate void failEventHandler();
	[Signal]
	public  delegate void change_moneyEventHandler(int money);
	[Signal]
	public  delegate void pick_unitEventHandler();
	[Signal]
	public  delegate void destroyed_townEventHandler(Node2D node);
	[Signal]
	public  delegate void take_damageEventHandler(Node2D node, Bullet bullet);
	[Signal]
	public  delegate void card_clickEventHandler();
	[Signal]
	public  delegate void pig_main_menu_animEventHandler();
	[Signal]
	public  delegate void havent_moneyEventHandler();

	public Node2D temp_pick_unit;		
	Timer t;
	public bool block_input;
	public bool invertY;
	public int money;
	public bool block_drop_unit;
	public List<PackedScene> GameLevels = new List<PackedScene>
	{
		ResourceLoader.Load<PackedScene>("res://scene/trenirovka.tscn")
	};
	
	
	
	public override void _Ready()
	{
		Instance = this;
		
	}
	
	
	
}
