using Godot;
using System;
using System.Threading.Tasks;
using GameObjects;

namespace TanksUtilits;
public static partial class GamaUtilits
{
    public static void shoot (Vector2 tank_pos, Vector2 marker_pos, Node i,float angle_pushka, Vector2 sc, int damage, int invertY, bool pushka_inside, float speed = 450.5f)
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
		b.angle_pushka = angle_pushka;
        b.pushka_inside = pushka_inside;
        i.GetTree().CurrentScene.AddChild(bullet);
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
        if(parent.GetChildren().Count == 0)
        {
            return;
        }
        foreach(var child in parent.GetChildren())
        {
            if(child is Sprite2D sprite)
            {
                ShaderMaterial shader = sprite.Material as ShaderMaterial;
                if(shader != null)shader.SetShaderParameter(name_param, _render);
            }
            set_shader(child, _render, name_param);

        }
    }
    public static async void  EnteredBulletInTownZone(Node2D body, PhysicsBody2D obj, Area2D area = null)
    {
        if((!body.IsInGroup("enemy") && !body.IsInGroup("unit")  && !body.IsInGroup("bullet")) || body == obj)return;
        if(obj is ITower  tower)
        {
            string name_group = string.Empty;
            if(obj.IsInGroup("unit")) name_group = "unit";
            else name_group = "enemy";
            if(body.IsInGroup(name_group))return;
            var body_pos = body.GlobalPosition;
            var space = obj.GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(tower.logic.pushka.GlobalPosition, body.GlobalPosition);
            query.Exclude = new Godot.Collections.Array<Rid> { obj.GetRid() };
            var result = space.IntersectRay(query);
            if(result.Count == 0)return;
            Node2D obstacle = (Node2D)result["collider"];
            if(obstacle.IsInGroup("well") || obstacle.IsInGroup(name_group))
            {
                return;
            }
            
            if(tower.logic.patron > 0 && tower.logic.can_shoot)
            {
                if(body is Bullet bullet)
                {
                    if(bullet.ray_cast_town.IsColliding())
                    {
                        var collider = (Node2D)bullet.ray_cast_town.GetCollider();
                        if(collider == null)return;
                        if(!(collider == obj))return;
                    }
                    else return;
                }
                var tween = obj.CreateTween();
                tween.SetEase(Tween.EaseType.InOut);
                tween.SetTrans(Tween.TransitionType.Sine);
                tween.TweenProperty(tower.logic.pushka, "rotation", (tower.logic.tower.GlobalPosition - body_pos).Normalized().Angle() - tower.logic.tower.GlobalRotation, tower.logic.time_tween);
                await obj.ToSignal(tween, "finished");
                tower.Shoot();
                if(area == null)return;
                EnteredBulletInTownZone(body, obj, area);
            }
            
            else if(tower.logic.patron >= 1 && !tower.logic.can_shoot)
            {
                if(area == null)return;
                await obj.ToSignal(tower.logic.t, "timeout");
                bool body_in_area = false;
                foreach (var node in area.GetOverlappingBodies())
                {
                    if(node  == body)
                    {
                        body_in_area = true;
                    }
                }
                if(!body_in_area)
                {
                    return;
                }
                
                EnteredBulletInTownZone(body, obj, area);
                
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
    public static void UnSetCollision(Node obj)
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
