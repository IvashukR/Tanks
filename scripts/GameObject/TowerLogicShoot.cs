using Godot;
using System;
using TanksUtilits;

namespace GameObjects;

public partial class TowerLogicShoot : BaseTowerLogic
{
    public bool can_shoot = true;
    public float time_tween = 0.2f;
    public int patron = 20;
    public Timer t { get; set; }
    public Sprite2D pushka;
    private Marker2D marker;
    [Export] public Vector2 bullet_size { get; set; } = new Vector2(0.165f, 0.171f);
    [Export] public int bullet_damage { get; set; } = 70;
    
	public override void _Ready()
	{
        pushka = GetParent<Sprite2D>();
        t = GetNode<Timer>("t_shoot");
        t.Timeout += () => can_shoot = true;
        marker = GetNode<Marker2D>("marker");
        base._Ready();
	}

	public void Shoot()
    {
        if(can_shoot && patron > 0)
        {
            can_shoot = false;
            t.Start();
            patron--;
            GamaUtilits.shoot(pushka.GlobalPosition, marker.GlobalPosition, tower, false,pushka.Rotation, bullet_size, bullet_damage, -1);
        }
    }
    

    

}
