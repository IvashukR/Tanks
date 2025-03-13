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
	public  delegate void winEventHandler();
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
	[Signal]
	public  delegate void cant_pick_unitEventHandler();
	public  bool fps;

	public Node2D temp_pick_unit;
	[Signal]
	public  delegate void _fpsEventHandler(bool value);		
	Timer t;
	public bool block_input;
	public int money;
	public bool block_drop_unit;
	public int last_level = 0;
	public List<string> PathLevels = new List<string>
	{
		"res://scene/trenirovka.tscn",
		"res://scene/level.tscn"
	};
	
	
	
	public override void _Ready()
	{
		Instance = this;
		
	}
	
	
	
}
