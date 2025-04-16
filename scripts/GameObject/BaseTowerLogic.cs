using Godot;
using System;
using TanksUtilits;

namespace GameObjects;

public partial class BaseTowerLogic : Node2D
{
    [Export] private bool main, player;
    [Export] public PhysicsBody2D tower;
    [Export] public int proch;
    public int max_proch;
    public CpuParticles2D blam_particles;
	public override void _Ready()
	{
        blam_particles = GetParent().GetParent().GetNode<CpuParticles2D>("blam");
        max_proch = proch;
	}

	
    public virtual void TakeDamage(int damage)
	{
        proch -= damage;
		GamaUtilits.DestroyTown(proch,blam_particles, tower);
		if(!main)return;
        string name_signal = string.Empty;
        if(!player)name_signal = "win";
        else name_signal = "fail";
		if(proch <= 0)GlobalManager.Instance.EmitSignal(name_signal);
	}

    

}
