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
    private Area2D bullet_detected;
    public override void _Ready()
    {
        bullet_detected = GetNode<Area2D>("%area_enemy_town");
        pushka = GetNode<Sprite2D>("%pushka_e");
        t = GetNode<Timer>("%t_enemy_town");
        t.Timeout += () => can_shoot = true;
        marker = GetNode<Marker2D>("%marker_enemy_town");
        bullet_detected.BodyEntered += (body) => {
            GamaUtilits.EnteredBulletInTownZone(body, this);
        };
        base._Ready();
        pushka.Rotation = (Position - GetNode<CharacterBody2D>("%Bullet").Position).Angle();
    }
    

}
