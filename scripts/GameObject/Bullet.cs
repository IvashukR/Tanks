using Godot;
using System;
using System.Reflection;
using TanksUtilits;
using GameUnit;

namespace GameObjects;
public partial class Bullet : CharacterBody2D
{
	public Vector2 mouse_pos;
	public Vector2 velocity;
	public Vector2 player_pos;
	public int damage = 50;
	public int ricoshet_count = 3;
	public bool coliide = true;
	[Export] public float speed = 80;
	public Vector2 dir;
	private Timer col;
	public bool fallow_m;
	public float angle_pushka;
	private Timer c;
	public int invertY;
	public RayCast2D ray_cast_town;
	private bool calculete_damage;
	private CpuParticles2D cpu_particles;
	private ShaderMaterial sm_blast;
	private AudioStreamPlayer audio_ricoshet;
	private AudioStreamPlayer audio_blast;

	public override void _Ready()
	{
		cpu_particles = GetNode<CpuParticles2D>("%blast");
		audio_blast = GetNode<AudioStreamPlayer>("%audio_blast");
		sm_blast = cpu_particles.Material as ShaderMaterial;
		audio_ricoshet = GetNode<AudioStreamPlayer>("%audio_ricoshet");
		ray_cast_town = GetNode<RayCast2D>("%ray_cast");
		mouse_pos = GetGlobalMousePosition();
		UpdDir();
		col = new Timer();
		AddChild(col);
		col.OneShot = true;
		col.WaitTime = 0.3;
		col.Timeout += () => coliide = true;
		c = GetNode<Timer>("%t");
		c.Timeout +=  () => CollisionMask = 1 | 2;
	}
	public Bullet(int damage)
	{
		this.damage = damage;
	}
	public Bullet()
    {
        
    }

	private void UpdDir()
	{
		if (fallow_m)
		{
			dir = (mouse_pos - player_pos).Normalized();
			Rotation = dir.Angle();
		}
		else
		{
			dir = new Vector2(Mathf.Cos(angle_pushka), Mathf.Sin(angle_pushka)).Normalized() * invertY;
			Rotation = dir.Angle();
		}
	}
	public override void _Process(double delta)
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
		
		if (body == null || body.IsQueuedForDeletion() || body == GlobalManager.Instance.temp_pick_unit)
    	{
        	return; 
    	}
		if (body.Name == "TankRed")
		{
			GlobalManager.Instance.EmitSignal("del_tank");
			_QueueFree();
		}
		if(body.Name == "bomba")
		{
			_QueueFree();
		}
		else if (body.IsInGroup("bullet"))
		{
			if(!body.IsQueuedForDeletion())body.QueueFree();
			_QueueFree();
		}


		else if (body.IsInGroup("well"))
		{
			var well = body as Well;
			if (well != null)
			{
				int currentProch = well.Proch;
				int newProch = currentProch - damage;
				well.Proch = newProch;
			}
			
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
		else if (body.IsInGroup("transport"))
		{
			
			int proch;
			var field = body.GetType().GetField("proch", BindingFlags.Public | BindingFlags.Instance);
			if (field != null && !calculete_damage)
            {
				calculete_damage = true;
				proch = (int)field.GetValue(body);
				field.SetValue(body, proch - damage);
				if (body.Name == "town")
				{
					GlobalManager.Instance.EmitSignal("del_t");
				}
				if(body.IsInGroup("town"))
				{
					GlobalManager.Instance.EmitSignal("destroyed_town", body);
				}
				_QueueFree();
			}
		}
		else if(body.IsInGroup("unit"))
		{
			if(body.GetNodeOrNull("%logic") is UnitLogic unit)unit.TakeDamage(this);
			_QueueFree();
		}
	}
	private void _QueueFree()
	{
		audio_blast.Play();
		GamaUtilits.DestroyTown(0,cpu_particles, this, sm_blast);
	}
}