using Godot;
using System;

public partial class Town1 : Town
{
    private bool is_ai = false;
    public TextureButton on_ai;
    private RayCast2D ray_cast;
    public override void _Ready()
    {
        ray_cast = GetNode<RayCast2D>("%raycast");
        on_ai = GetNode<TextureButton>("%on_ai");
        on_ai.Pressed += () => 
        {
            is_ai = !is_ai;
        };
        base._Ready();
        
    }
    public void set_outline(bool _render)
    {
        foreach(var child in GetChildren())
        {
            if(child is Sprite2D sprite)
            {
                ShaderMaterial shader = sprite.Material as ShaderMaterial;
                shader.SetShaderParameter("render", _render);
            }
            foreach(var _child in child.GetChildren())
            {
                if(_child is Sprite2D _sprite)
                {
                    ShaderMaterial shader = _sprite.Material as ShaderMaterial;
                    shader.SetShaderParameter("render", _render);
                }
                
            }

        }
    }
    public override void _Input(InputEvent @event)
    {
        if(!is_ai)base._Input(@event);
    }
    public override async void  _Process(double delta)
    {
        if(!is_ai)
        {
            base._Process(delta);
        }
        else
        {
            ray_cast.Rotation += 1.3f;
            if(ray_cast.IsColliding())
            {
                var collider = ray_cast.GetCollider();
                if(collider is Node coll)
                {
                    if(coll.IsInGroup("bullet"))
                    {
                        var tween = CreateTween();
                        tween.SetEase(Tween.EaseType.InOut);
                        tween.SetTrans(Tween.TransitionType.Sine);
                        tween.TweenProperty(pushka, "rotation", (ray_cast.Rotation - Mathf.DegToRad(90)) * -1, 1.5f);
                        await ToSignal(tween, "finished");
                        GlobalManager.Instance.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false, false, pushka.Rotation, new Vector2(0.165f, 0.171f), 50);
                    }
                }

            }
        }
    }
}
