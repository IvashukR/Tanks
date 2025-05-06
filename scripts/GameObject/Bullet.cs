using Godot;
using System;
using TanksUtilits;

namespace GameObjects;
public partial class Bullet : CharacterBody2D, IMoveble
{
	public Vector2 mouse_pos;
	public Vector2 velocity;
	public Vector2 player_pos;
	public int damage = 50;
	public int ricoshet_count = 3;
	public bool coliide = true, pushka_inside;
	[Export] public float speed {set;get;} = 80;
	public Vector2 dir;
	private Timer col;
	public float angle_pushka;
	public int invertY;
	public RayCast2D ray_cast_town;
	private bool calculete_damage;
	private CpuParticles2D cpu_particles;
	private ShaderMaterial sm_blast;
	private AudioStreamPlayer audio_ricoshet;
	private AudioStreamPlayer audio_blast;
	private Area2D area_collide;

	public override void _Ready()
	{
		cpu_particles = GetNode<CpuParticles2D>("%blast");
		audio_blast = GetNode<AudioStreamPlayer>("%audio_blast");
		sm_blast = cpu_particles.Material as ShaderMaterial;
		area_collide = GetNode<Area2D>("%area_collide");
		audio_ricoshet = GetNode<AudioStreamPlayer>("%audio_ricoshet");
		ray_cast_town = GetNode<RayCast2D>("%ray_cast");
		mouse_pos = GetGlobalMousePosition();
		area_collide.AreaEntered += AreaCollideEnteredArea;
		UpdDir();
		col = GetNode<Timer>("%t");
		col.Timeout += () => coliide = true;
		if(pushka_inside)
		{
			var collision_layer = CollisionLayer;
			var collision_mask = CollisionMask;
			CollisionLayer = 0;
			CollisionMask = 0;
			var c = new Timer();
			c.Autostart = true;
			c.OneShot = true;
			c.WaitTime = 0.1;
			AddChild(c);
			c.Timeout +=  () => {
				CollisionLayer = collision_layer;
				CollisionMask = collision_mask;
			};
		}
	}
	

	private void UpdDir()
	{
		dir = new Vector2(Mathf.Cos(angle_pushka), Mathf.Sin(angle_pushka)).Normalized() * invertY;
		Rotation = dir.Angle();
	}
	private void AreaCollideEnteredArea(Area2D area)
	{
		if(area.IsInGroup("bomba"))
		_QueueFree();
	}
	public override void _PhysicsProcess(double delta)
	{
		
		velocity = dir * speed * (float)delta;

        
        var collision = MoveAndCollide(velocity);
		
		if (collision != null)
        {
            var collider = collision.GetCollider() as Node2D;
            if (collider != null && coliide) 
            {
                entered(collider, collision.GetNormal());
				coliide = false;
				col.Start();
            }
        }
		

		Vector2 rect = GetViewport().GetVisibleRect().Size;
		if (GlobalPosition.X < 0 || GlobalPosition.X > rect.X || GlobalPosition.Y < 0 || GlobalPosition.Y > rect.Y)
		{
			QueueFree();
		}
		
		
	}
	private  void entered(Node2D body, Vector2 normal)
	{
		if(body is IDamageble actor)
		{
			actor.TakeDamage(this.damage);
			_QueueFree();
		}
		
		if (body.IsInGroup("bullet"))
		{
			if(!body.IsQueuedForDeletion())body.QueueFree();
			_QueueFree();
		}


		else if (body.IsInGroup("well"))
		{
			
    		dir = dir - 2 * dir.Dot(normal) * normal;
			if(ricoshet_count - 1 > 0)
			if(!audio_ricoshet.IsPlaying())audio_ricoshet.Play();
			Rotation = dir.Angle();
			ricoshet_count--;
			if (ricoshet_count <= 0)
			{
				_QueueFree();
			}
		}
		
	}
	private void _QueueFree()
	{
		audio_blast.Play();
		GamaUtilits.DestroyTown(0,cpu_particles, this);
	}
	public override void _ExitTree()
	{
		col.QueueFree();
	}
}