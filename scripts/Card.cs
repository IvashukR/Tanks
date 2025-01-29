using Godot;
using System;

public partial class Card : Control
{
	private Label money_l = new Label();
	private Label speed_l = new Label();
	private Label per_l = new Label();
	private Label d_cost_l = new Label();
	private Label damage_l = new Label();
	private Label patron_l = new Label();
	private int cost = 0;
	private VBoxContainer info;
	private TextureButton show_i;
	private TextureButton hide_i;
	private TextureButton main_btn;
	[Export] public string _path;

	public void SetInfo(object obj)
	{
		if (obj is IStats target)
		{
			money_l.Text = $": {target.cost}";
			patron_l.Text = $"патрони: {target.patron_count}";
			speed_l.Text = $"скорость: {target.speed}";
			per_l.Text = $"перезарядка: {target.perezaryad}";
			d_cost_l.Text = $"цена смерти: {target.cost_death}";
			damage_l.Text = $"урон: {target.damage}";
			cost = target.cost;
		}
	}
	
	public void Buy(string _path)
	{
		PackedScene s  = ResourceLoader.Load<PackedScene>(this._path);
		var sc = s.Instantiate<Node2D>();
		Node fsm_node = sc.GetNode("%FSM");
		FSM fsm = fsm_node as FSM;
		sc.GlobalPosition = GlobalPosition;
		AddChild(sc);
		fsm.change_state("Void");
	}
	public override void _Ready()
	{
		main_btn = GetNode<TextureButton>("%main_btn");
		info = GetNode<VBoxContainer>("%info");
		show_i = GetNode<TextureButton>("%show_i");
		hide_i = GetNode<TextureButton>("%hide_i");
		money_l = GetNode<Label>("%money_l");
		speed_l = GetNode<Label>("%speed_l");
		per_l = GetNode<Label>("%per_l");
		d_cost_l = GetNode<Label>("%death_l");
		patron_l = GetNode<Label>("%patron_l");
		damage_l = GetNode<Label>("%damage_l");
		main_btn.Pressed += () => Buy(_path);
		show_i.Pressed += () => {
			info.Visible = true;
			show_i.Visible = false;
			hide_i.Visible = true;
		};
		hide_i.Pressed += () => {
			info.Visible = false;
			hide_i.Visible = false;
			show_i.Visible = true;
		};
		GlobalManager.Instance.change_money += () => GlobalManager.Instance.Money -= cost;

	}
	public override void _ExitTree()
{
    
    foreach (Node child in GetChildren())
    {
        if (child is CanvasItem)
        {
            child.QueueFree();
        }
    }
}


	
	
}
