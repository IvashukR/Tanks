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
	public void Buy(int cost)
	{
		//GlobalManager.Instance.money
	}
	public override void _Ready()
	{
		money_l = GetNode<Label>("%money_l");
		speed_l = GetNode<Label>("%speed_l");
		per_l = GetNode<Label>("%per_l");
		d_cost_l = GetNode<Label>("%death_l");
		damage_l = GetNode<Label>("%damage_l");


	}

	
	public override void _Process(double delta)
	{
	}
}
