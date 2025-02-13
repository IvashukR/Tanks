using Godot;
using System;

public partial class TownEnemy : StaticBody2D
{
	public int proch = 50;
	private bool is_boom;
	private ShaderMaterial sm;
	private CpuParticles2D blam_particles;
    private Label hp_l;
    private BoxContainer info;
	public override void _Ready()
	{
        hp_l = GetNode<Label>("%hp_l_enemy_town");
        info = GetNode<BoxContainer>("%info_enemy_town");
		blam_particles = GetNode<CpuParticles2D>("%blast");
		sm = blam_particles.Material as ShaderMaterial;
		GlobalManager.Instance.destroyed_enemy_town += () => GamaUtilits.DestroyTown(proch, is_boom, blam_particles, this, sm);
	}


}
