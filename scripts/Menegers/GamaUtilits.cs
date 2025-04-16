using Godot;
using System;
using System.Threading.Tasks;
using GameObjects;

namespace TanksUtilits;
public static partial class GamaUtilits
{
    public static void shoot (Vector2 tank_pos, Vector2 marker_pos, Node i, bool fallow_m, float angle_pushka, Vector2 sc, int damage, int invertY, float speed = 450.5f)
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
        
	}
	public static void spawn_d (Vector2 pos, Vector2 sc)
	{
		var _dialog = (PackedScene)ResourceLoader.Load("res://scene/dialog.tscn");
		var dialog = _dialog.Instantiate<CharacterBody2D>();
		dialog.GlobalPosition = pos;
		dialog.Scale = sc;
	}
    public static bool CheckRayCollide(RayCast2D ray, string group)
    {
        if(ray.IsColliding())
        {
            var collider = (Node2D)ray.GetCollider();
            if(collider == null)return false;
            if(collider.IsInGroup(group))return true;
        }
        return false;
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
    public static async void  EnteredBulletInTownZone(Node2D body, Node2D obj)
    {
        if(obj is ITower  tower)
        {
            if(tower.logic.patron > 0 && tower.logic.can_shoot)
            {
                Vector2 future_pos = new Vector2();
                if(body == GlobalManager.Instance.temp_pick_unit)return;
                if(body is Bullet bullet)
                {
                    if(bullet.ray_cast_town.IsColliding())
                    {
                        var collider = (Node2D)bullet.ray_cast_town.GetCollider();
                        if(collider == null)return;
                        if(collider.IsInGroup("transport"))future_pos = bullet.Position;
                        else return;
                    }
                    else return;
                }
                else if(body.IsInGroup("unit") && !obj.IsInGroup("unit"))future_pos = body.GlobalPosition;
                else return;
                var tween = obj.CreateTween();
                tween.SetEase(Tween.EaseType.InOut);
                tween.SetTrans(Tween.TransitionType.Sine);
                tween.TweenProperty(tower.logic.pushka, "rotation", (tower.logic.tower.GlobalPosition - future_pos).Normalized().Angle(), tower.logic.time_tween);
                await obj.ToSignal(tween, "finished");
                string name_group = string.Empty;
                if(obj.IsInGroup("unit")) name_group = "unit";
                else name_group = "enemy"; 
                if(CheckRayCollide(tower.ray_attack, "well") || CheckRayCollide(tower.ray_attack, name_group))return;
                tower.Shoot();
            }
        }
    }

    public static async void DestroyTown(int proch,CpuParticles2D blam_particles, Node2D obj)
    {
        if(proch <= 0)
        {
            await DestroyObjectParticles(obj, blam_particles, true);
        }
        else
        {
            set_shader(obj, true, "damage");
            await obj.ToSignal(obj.GetTree().CreateTimer(0.2f), "timeout");
            set_shader(obj, false, "damage");
        }

        
    }
    private static void UnSetCollision(Node obj)
    {
        if(obj is PhysicsBody2D body)
        {
            body.CollisionLayer = 0;
            body.CollisionMask = 0;
        }
    }
    public static async Task DestroyObjectParticles(Node2D obj, CpuParticles2D blam, bool boom)
    {
        blam.Emitting = true;
        UnSetCollision(obj);
        if(obj is IMoveble body)
        {
            body.speed = 0.0f;
        }
        if(boom)
        {
            var blam_sm = blam.Material as ShaderMaterial;
            for (float i = 0.0f; i <= 1; i += 0.3f)
            {
			    await obj.ToSignal(obj.GetTree().CreateTimer(0.03f), "timeout");
                blam_sm.SetShaderParameter("glow_strength", i);
            }
        }
        foreach(var child in obj.GetChildren())
        {
            UnSetCollision(child);
            if(child is CanvasItem node && !node.IsInGroup("boom"))
            {
                node.Hide();
            }
            else if(child is CanvasLayer hud)
            {
                foreach(CanvasItem item in hud.GetChildren())
                {
                    item.Hide();
                }
            }
        }
        await obj.ToSignal(obj.GetTree().CreateTimer(blam.Lifetime), "timeout");
        obj.QueueFree();
    }
    

}
