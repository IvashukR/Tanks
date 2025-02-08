using Godot;
using System;

public partial class TownEnemy : StaticBody2D
{
	public int proch = 50;
	private bool is_boom;
	private ShaderMaterial sm;
	private CpuParticles2D blam_particles;
	public override void _Ready()
	{
		blam_particles = GetNode<CpuParticles2D>("%blast");
		sm = blam_particles.Material as ShaderMaterial;
		GlobalManager.Instance.destroyed_enemy_town += Destroy;
	}

    private async void Destroy()
    {
        if(proch <= 0)
        {
            if(is_boom)return;
            is_boom = true;
            blam_particles.Emitting = true;
            for (float i = 0.0f; i <= 1; i += 0.3f)
            {
			    await ToSignal(GetTree().CreateTimer(0.19f), "timeout");
                sm.SetShaderParameter("glow_strength", i);
            }
            QueueFree();
        }
        else
        {
            Town1.set_shader(this, true, "damage");
            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
            Town1.set_shader(this, false, "damage");
        }

        
    }
	public override void _ExitTree()
	{
		GlobalManager.Instance.destroyed_enemy_town -= Destroy;
	}

}
