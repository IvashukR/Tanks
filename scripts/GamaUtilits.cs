using Godot;
using System;

public partial class GamaUtilits : Node
{
    public static void shoot (Vector2 tank_pos, Vector2 marker_pos, Node i, bool particl, bool fallow_m, float angle_pushka, Vector2 sc, int damage, int invertY)
	{
		var _bullet = (PackedScene)ResourceLoader.Load("res://scene/bullet.tscn");
		var bullet = _bullet.Instantiate<CharacterBody2D>();
		bullet.GlobalPosition = marker_pos;
		bullet.Scale = sc;
		var b = bullet as Bullet;
		b.damage = damage;
		b.player_pos = tank_pos;
        b.invertY = invertY;
		b.fallow_m = fallow_m;
		b.angle_pushka = angle_pushka;
		i.GetParent().AddChild(bullet);
		b.p.Emitting = particl;
		

	}
	public static void spawn_d (Vector2 pos, Vector2 sc)
	{
		var _dialog = (PackedScene)ResourceLoader.Load("res://scene/dialog.tscn");
		var dialog = _dialog.Instantiate<CharacterBody2D>();
		dialog.GlobalPosition = pos;
		dialog.Scale = sc;
	}
    public static void set_shader(Node parent, bool _render, string name_param)
    {
        foreach(var child in parent.GetChildren())
        {
            if(child is Sprite2D sprite)
            {
                ShaderMaterial shader = sprite.Material as ShaderMaterial;
                shader.SetShaderParameter(name_param, _render);
            }
            foreach(var _child in child.GetChildren())
            {
                if(_child is Sprite2D _sprite)
                {
                    ShaderMaterial shader = _sprite.Material as ShaderMaterial;
                    shader.SetShaderParameter(name_param, _render);
                }
                
            }

        }
    }
    public static async void  EnteredBulletInTownZone(Node2D body, Node2D obj)
    {
        if(body is Bullet bullet)
        {
                if(obj is ITown  town)
                {
                    if(town.patron > 0 && town.can_shoot)
                    {
                        Vector2 future_pos = bullet.GlobalPosition + (bullet.dir * bullet.speed * town.bullet_area_koef);
                        var tween = obj.CreateTween();
                        tween.SetEase(Tween.EaseType.InOut);
                        tween.SetTrans(Tween.TransitionType.Sine);
                        if(obj is CharacterBody2D _town)tween.TweenProperty(town.pushka, "rotation", (_town.GlobalPosition - future_pos).Normalized().Angle(), town.time_tween);
                        await obj.ToSignal(tween, "finished");
                        GamaUtilits.shoot(town.pushka.GlobalPosition, town.marker.GlobalPosition, obj, false, false, town.pushka.Rotation, town.bullet_size, town.bullet_damage, -1);
                        town.can_shoot = false;
                        town.t.Start();
                        town.patron--;
                        town.patron_l.Text = $"Town patron: {town.patron}";
                    }
                }
        }
    }

}
