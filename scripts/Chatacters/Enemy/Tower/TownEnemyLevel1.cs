using Godot;
using System;
using TanksUtilits;
using GameObjects;
using Player.Transport;

namespace Enemy.Transport;

public partial class TownEnemyLevel1 : TownEnemy, ITown
{
    public bool can_shoot { get; set; } = true;
    public float time_tween { get; set; } = 0.2f;
    public int patron { get; set; } = 20;
    public Timer t { get; set; }
    public Sprite2D pushka { get; set; }
    public Marker2D marker { get; set; }
    [Export] public Vector2 bullet_size { get; set; } = new Vector2(0.165f, 0.171f);
    [Export] public int bullet_damage { get; set; } = 70;

    private Area2D bullet_detected, unit_detected;
    private float last_time_entered_unit;
    private bool flag_unit = true, flag_attacked;
    private Timer t_unit, t_attack, t_flag_attack;
    private Well hit_well;
    private RandomNumberGenerator rng;
    public RayCast2D ray_attack { get; set; }
    private Town tower;
    [Export] private bool attacked = true;
    public override void _Ready()
    {
        tower = GetNode<Town>("%town");
        ray_attack = GetNode<RayCast2D>("%ray_attack");
        rng = new RandomNumberGenerator();
        rng.Randomize();
        t_attack = GetNode<Timer>("%t_attack");
        t_flag_attack = GetNode<Timer>("%t_flag_attack");
        t_unit = GetNode<Timer>("%t_unit");
        bullet_detected = GetNode<Area2D>("%area_enemy_town");
        unit_detected = GetNode<Area2D>("%area_enemy_town_unit");
        pushka = GetNode<Sprite2D>("%pushka_e");
        t = GetNode<Timer>("%t_enemy_town");
        t.Timeout += () => can_shoot = true;
        marker = GetNode<Marker2D>("%marker_enemy_town");
        t_unit.Timeout += () => flag_unit = true;
        tower.TreeExited += () => tower = null;
        bullet_detected.BodyEntered += (body) => {
            GamaUtilits.EnteredBulletInTownZone(body, this, true);
        };
        unit_detected.BodyEntered += (body) => 
        {
            GamaUtilits.EnteredBulletInTownZone(body, this, false);
            last_time_entered_unit = Time.GetTicksMsec() / 1000;
        };
        base._Ready();
        if(attacked)
        {
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
        t.WaitTime = 2;
        t_attack.Start();
        pushka.Rotation = (Position - GetNode<Marker2D>("%marker_attack").Position).Angle();
        for(int i = 0; i < 3; i++)
        {
            Shoot();
            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        }
    }
    protected void Shoot()
    {
        if(can_shoot && patron > 0)
        {
            can_shoot = false;
            t.Start();
            patron--;
            GamaUtilits.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false,pushka.Rotation, bullet_size, 50, -1);
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
                GamaUtilits.EnteredBulletInTownZone(unit_detected.GetOverlappingBodies()[random_body_index], this, false);
                flag_unit = false;
                t_unit.Start();
            }
        }
        if(GamaUtilits.CheckRayCollide(ray_attack, "unit"))
        {
            Shoot();
        }
    }
    

}
