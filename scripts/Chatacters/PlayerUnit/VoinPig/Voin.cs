using Godot;
using System;

public partial class Voin : CharacterBody2D
{
    [Export] private UnitLogic unit;
    [Export] private Vector2 scale_bullet = new Vector2(0.1f, 0.1f);
    [Export] private float speed_bullet = 498.9f;
    private Sprite2D pushka;
    private Marker2D marker;
    public bool can_shoot = true;
    private Label patron_l;
    private Timer t_shoot;
    

    public override void _Ready()
    {
        pushka = GetNode<Sprite2D>("%pushka_voin");
		marker = GetNode<Marker2D>("%marker");
        patron_l = GetNode<Label>("%patron_l");
        t_shoot = new Timer();
		t_shoot.WaitTime = unit.stats.perezaryad;
		t_shoot.OneShot = true;
        t_shoot.Timeout += () => can_shoot = true;
        AddChild(t_shoot);
        t_shoot.TreeExited += () => t_shoot = null;
        patron_l.Text = $"{unit.name_unit} Patron: {unit.stats.patron_count}";
    }
    public void Shoot(bool mouse_fallow)
	{
        if(!can_shoot || unit.stats.patron_count <= 0)return;
		if(mouse_fallow)GamaUtilits.shoot(marker.GlobalPosition, marker.GlobalPosition, this, true, marker.Rotation, scale_bullet, unit.stats.damage , -1, speed_bullet);
        else GamaUtilits.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false, Rotation , scale_bullet ,unit.stats.damage, 1, speed_bullet);
		can_shoot = false;
		unit.stats.patron_count--;
		patron_l.Text = $"{unit.name_unit} Patron: {unit.stats.patron_count}";
		t_shoot.Start();
	}
    public bool CheckAmmoNull()
    {
        return unit.stats.patron_count <= 0;
    }
}
