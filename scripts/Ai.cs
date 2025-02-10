using Godot;
using System;

public partial class Ai : State
{
	private bool curved_path_clear = true;
	private Voin voin;
	private bool ray_flag = true;
	private Timer ray_timer;
	private RayCast2D detect_enemy_ray;
	private Marker2D marker;
	public override void _Ready()
	{
		marker = GetNode<Marker2D>("%marker");
		ray_timer = GetNode<Timer>("%ray_timer");
		voin = GetParent().GetParent<Voin>();
		detect_enemy_ray = GetNode<RayCast2D>("%ray_ai");
		ray_timer.Timeout += () => ray_flag = true;
	}

	public override void Process(double delta)
	{
		if(curved_path_clear)CheckUnitCollideNotCurved();
	}
	private void CheckUnitCollideNotCurved()
	{
		if(voin.patron_count > 0)
		{
			voin.Rotation += 0.04f;
			if(detect_enemy_ray.IsColliding() && ray_flag)
			{
				ray_flag = false;
				Node2D collider = (Node2D)detect_enemy_ray.GetCollider();
				ray_timer.Start();
				if(collider.IsInGroup("enemy") || collider.IsInGroup("bullet"))
				{
					voin.LookAt(collider.GlobalPosition);
					GlobalManager.Instance.shoot(marker.GlobalPosition, marker.GlobalPosition, this, false, false, marker.Rotation + Mathf.DegToRad(90), new Vector2(0.1f, 0.1f), voin.damage);
					voin.patron_count--;
				}

			}
		}
	}
	
}

