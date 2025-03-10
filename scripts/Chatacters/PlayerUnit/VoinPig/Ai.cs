using Godot;
using System;

public partial class Ai : State
{
	private bool curved_path_clear = true;
	[Export] private CharacterBody2D voin;
	[Export] private UnitLogic unit;
	private bool ray_flag = true;
	private Timer ray_timer;
	private Sprite2D pushka;
	private RayCast2D detect_enemy_ray;
	private Marker2D marker;
	public override void _Ready()
	{
		pushka = GetNode<Sprite2D>("%pushka_voin");
		marker = GetNode<Marker2D>("%marker");
		ray_timer = GetNode<Timer>("%ray_timer");
		detect_enemy_ray = GetNode<RayCast2D>("%ray_ai");
		ray_timer.Timeout += () => ray_flag = true;
	}

	public override void Process(double delta)
	{
		if(curved_path_clear)CheckUnitCollideNotCurved();
	}
	private void CheckUnitCollideNotCurved()
	{
		if(unit.stats.patron_count > 0)
		{
			voin.Rotation += 0.04f;
			if(detect_enemy_ray.IsColliding() && ray_flag)
			{
				//ray_flag = false;
				Node2D collider = (Node2D)detect_enemy_ray.GetCollider();
				if(collider == null)return;
				ray_timer.Start();
				if(collider.IsInGroup("enemy") || collider.IsInGroup("bullet"))
				{
					ray_flag = false;
					voin.LookAt(collider.GlobalPosition);
					GamaUtilits.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false,voin.Rotation , new Vector2(0.1f, 0.1f), unit.stats.damage, 1, 500);
					unit.stats.patron_count--;
				}

			}
		}
	}
	
}

 