using Godot;
using System;

public partial class TownEnemyLevel1 : TownEnemy, ITown
{
    public bool can_shoot { get; set; } = true;
    public float time_tween { get; set; } = 0.2f;
    public int patron { get; set; } = 10;
    public Timer t { get; set; }
    public Sprite2D pushka { get; set; }
    public Marker2D marker { get; set; }
    [Export] public Vector2 bullet_size { get; set; } = new Vector2(0.165f, 0.171f);
    [Export] public int bullet_damage { get; set; } = 70;

    [Export] public float bullet_area_koef { get; set; } = 0.1f;
    private Area2D bullet_detected, unit_detected;
    private float last_time_entered_unit;
    private bool flag_unit = true;
    private Timer t_unit;
    public override void _Ready()
    {
        t_unit = GetNode<Timer>("%t_unit");
        bullet_detected = GetNode<Area2D>("%area_enemy_town");
        unit_detected = GetNode<Area2D>("%area_enemy_town_unit");
        pushka = GetNode<Sprite2D>("%pushka_e");
        t = GetNode<Timer>("%t_enemy_town");
        t.Timeout += () => can_shoot = true;
        marker = GetNode<Marker2D>("%marker_enemy_town");
        t_unit.Timeout += () => flag_unit = true;
        bullet_detected.BodyEntered += (body) => {
            GamaUtilits.EnteredBulletInTownZone(body, this, true);
        };
        unit_detected.BodyEntered += (body) => 
        {
            GamaUtilits.EnteredBulletInTownZone(body, this, false);
            last_time_entered_unit = Time.GetTicksMsec() / 1000;
        };
        base._Ready();
        //pushka.Rotation = (Position - GetNode<CharacterBody2D>("%Bullet").Position).Angle();
    }
    public override void _PhysicsProcess(double delta)
    {
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
    }
    

}
