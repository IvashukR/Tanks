using Godot;
using System;
using TanksUtilits;
using GameObjects;
using Player.Transport;

namespace Enemy.Transport;

public partial class TownEnemyLevel1 : TowerEnemy , ITower
{
    private Area2D bullet_detected, unit_detected;
    private float last_time_entered_unit;
    private bool flag_unit = true, flag_attacked = true;
    private Timer t_unit, t_attack, t_flag_attack;
    private Well hit_well;
    private RandomNumberGenerator rng;
    private Town tower;
    [Export] private bool attacked = true;
    [Export] public RayCast2D ray_attack {set;get;}
    [Export] public  TowerLogicShoot logic  {set;get;}
    public override void _Ready()
    {
        tower = GetNode<Town>("%town");
        rng = new RandomNumberGenerator();
        rng.Randomize();
        t_unit = GetNode<Timer>("t_unit");
        bullet_detected = GetNode<Area2D>("area_town");
        unit_detected = GetNode<Area2D>("area_town_unit");
        t_unit.Timeout += () => flag_unit = true;
        tower.TreeExited += () => tower = null;
        bullet_detected.BodyEntered += (body) => {
            GamaUtilits.EnteredBulletInTownZone(body, this);
        };
        unit_detected.BodyEntered += (body) => 
        {
            GamaUtilits.EnteredBulletInTownZone(body, this);
            last_time_entered_unit = Time.GetTicksMsec() / 1000;
        };
        base._Ready();
        if(attacked)
        {
            t_attack = GetNode<Timer>("t_attack");
            t_flag_attack = GetNode<Timer>("t_flag_attack");
            t_flag_attack.Timeout += () => flag_attacked = false;
            t_attack.WaitTime = rng.RandiRange(20, 50);
            t_attack.Timeout += Attack;
            hit_well = GetNode<Well>("%well");
            hit_well.TreeExited += () => hit_well = null;
        }
    }
    private async void Attack()
    {
        if(!attacked)return;
        if(tower == null)
        {
            attacked = false;
            return;
        }
        logic.t.WaitTime = 2;
        t_attack.Start();
        logic.pushka.Rotation = (Position - GetNode<Marker2D>("marker_attack").Position).Angle();
        for(int i = 0; i < 3; i++)
        {
            logic.Shoot();
            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        }
    }

    public override void _Process(double delta)
    {
        if(hit_well == null && !flag_attacked && tower != null)
        {
            flag_attacked = true;
            t_flag_attack.Start();
            Attack();
        }
        if(Time.GetTicksMsec() / 1000 - last_time_entered_unit > 0.4 && flag_unit)
        {
            if(unit_detected.GetOverlappingBodies().Count > 0)
            {
                int random_body_index = GD.RandRange(0, unit_detected.GetOverlappingBodies().Count - 1);
                GamaUtilits.EnteredBulletInTownZone(unit_detected.GetOverlappingBodies()[random_body_index], this);
                flag_unit = false;
                t_unit.Start();
            }
        }
        if(GamaUtilits.CheckRayCollide(ray_attack, "unit"))
        {
            Shoot();
        }
    }
    public virtual void Shoot()
    {
        logic.Shoot();
    }
    
    public override void TakeDamage(int damage)
	{
		logic.TakeDamage(damage);
		EmitSignal("change_hp");
	}


}
