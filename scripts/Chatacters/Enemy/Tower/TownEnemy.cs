using Godot;
using System;

public partial class TownEnemy : StaticBody2D
{
	public int proch = 50;
	public int max_proch;
	private bool is_boom;
	private ShaderMaterial sm;
	private CpuParticles2D blam_particles;
    private Label hp_l;
	public override void _Ready()
	{
		max_proch = proch;
		blam_particles = GetNode<CpuParticles2D>("%blast");
		sm = blam_particles.Material as ShaderMaterial;
		GlobalManager.Instance.destroyed_town += destroy;
	}
	private void destroy(Node2D node)
	{
		if(node == this)GamaUtilits.DestroyTown(proch, is_boom, blam_particles, this, sm);
	}
	public override void _ExitTree()
	{
		GlobalManager.Instance.destroyed_town -= destroy;
	}


}
