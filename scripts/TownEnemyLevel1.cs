using Godot;
using System;

public partial class TownEnemyLevel1 : TownEnemy, ITown
{
    private BoxContainer info;
    private Label hp_l;
    public bool can_shoot { get; set; } = true;
    public Label patron_l { get; set; }
    public float time_tween { get; set; } = 0.1f;
    public int patron { get; set; } = 10;
    public Timer t { get; set; }
    public Sprite2D pushka { get; set; }
    public Marker2D marker { get; set; }
    [Export] public Vector2 bullet_size { get; set; } = new Vector2(0.165f, 0.171f);
    [Export] public int bullet_damage { get; set; } = 70;

    [Export] public float bullet_area_koef { get; set; } = 0.1f;
    public override void _Ready()
    {

        base._Ready();
    }

}
