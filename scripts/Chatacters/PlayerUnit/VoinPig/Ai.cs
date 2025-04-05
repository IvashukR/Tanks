using Godot;
using System;

namespace Player.Actor;
public partial class Ai : State
{
	[Export] private Voin voin;
	private bool ray_flag = true;
	private Timer ray_timer;
	private RayCast2D detect_enemy_ray;
	public override void _Ready()
	{
		ray_timer = GetNode<Timer>("%ray_timer");
		detect_enemy_ray = GetNode<RayCast2D>("%ray_ai");
		ray_timer.Timeout += () => ray_flag = true;
	}

	public override void Process(double delta)
	{
		CheckUnitCollideNotCurved();
	}
	private void CheckUnitCollideNotCurved()
	{
		if(!voin.CheckAmmoNull())
		{
			voin.Rotation += 0.04f;
			if(detect_enemy_ray.IsColliding() && ray_flag)
			{
				Node2D collider = (Node2D)detect_enemy_ray.GetCollider();
				if(collider == null)return;
				ray_timer.Start();
				if(collider.IsInGroup("enemy") || collider.IsInGroup("bullet"))
				{
					ray_flag = false;
					voin.LookAt(collider.GlobalPosition);
					voin.Shoot(false);
				}

			}
		}
	}
	
}

 