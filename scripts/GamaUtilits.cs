using Godot;
using System;

public partial class GamaUtilits : Node
{
    public static void shoot (Vector2 tank_pos, Vector2 marker_pos, Node i, bool particl, bool fallow_m, float angle_pushka, Vector2 sc, int damage, int invertY, float speed = 450.5f)
	{
		var _bullet = (PackedScene)ResourceLoader.Load("res://scene/bullet.tscn");
		var bullet = _bullet.Instantiate<CharacterBody2D>();
		bullet.GlobalPosition = marker_pos;
		bullet.Scale = sc;
		var b = bullet as Bullet;
		b.damage = damage;
        b.speed = speed;
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
                    if(shader != null)shader.SetShaderParameter(name_param, _render);
                }
                
            }

        }
    }
    public static async void  EnteredBulletInTownZone(Node2D body, Node2D obj, bool _bullet)
    {
        if(obj is ITown  town)
        {
            if(town.patron > 0 && town.can_shoot)
            {
                Vector2 future_pos = new Vector2();
                if(body == GlobalManager.Instance.temp_pick_unit)return;
                if(body is Bullet bullet && _bullet)future_pos = bullet.GlobalPosition + (bullet.dir * bullet.speed * town.bullet_area_koef);
                else if(body.IsInGroup("unit") && !obj.IsInGroup("unit"))future_pos = body.GlobalPosition;
                else return;
                var tween = obj.CreateTween();
                tween.SetEase(Tween.EaseType.InOut);
                tween.SetTrans(Tween.TransitionType.Sine);
                tween.TweenProperty(town.pushka, "rotation", (obj.GlobalPosition - future_pos).Normalized().Angle(), town.time_tween);
                await obj.ToSignal(tween, "finished");
                GamaUtilits.shoot(town.pushka.GlobalPosition, town.marker.GlobalPosition, obj, false, false, town.pushka.Rotation, town.bullet_size, town.bullet_damage, -1);
                town.can_shoot = false;
                town.t.Start();
                town.patron--;
            }
        }
    }

    public static async void DestroyTown(int proch, bool is_boom, CpuParticles2D blam_particles, Node2D obj, ShaderMaterial blam_sm)
    {
        if(proch <= 0)
        {
            if(is_boom)return;
            is_boom = true;
            blam_particles.Emitting = true;
            for (float i = 0.0f; i <= 1; i += 0.3f)
            {
			    await obj.ToSignal(obj.GetTree().CreateTimer(0.19f), "timeout");
                blam_sm.SetShaderParameter("glow_strength", i);
            }
            GlobalManager.Instance.EmitSignal("fail");
            obj.QueueFree();
        }
        else
        {
            GamaUtilits.set_shader(obj, true, "damage");
            await obj.ToSignal(obj.GetTree().CreateTimer(0.2f), "timeout");
            GamaUtilits.set_shader(obj, false, "damage");
        }

        
    }

}
