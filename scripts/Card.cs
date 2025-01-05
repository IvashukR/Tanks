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
	[Export] public string _path;
	public void SetInfo(object obj)
	{
		if (obj is IStats target)
		{
			money_l.Text = $": {target.cost}";
			speed_l.Text = $"скорость: {target.speed}";
			per_l.Text = $"перезарядка: {target.perezaryad}";
			d_cost_l.Text = $"цена смерти: {target.perezaryad}";
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
		fsm.change_state("Void");
		sc.GlobalPosition = GlobalPosition;
		AddChild(sc);
	}
	public override void _Ready()
	{
		money_l = GetNode<Label>("%money_l");
		speed_l = GetNode<Label>("%speed_l");
		per_l = GetNode<Label>("%per_l");
		d_cost_l = GetNode<Label>("%death_l");
		damage_l = GetNode<Label>("%damage_l");
		SetInfo(new Voin());

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
