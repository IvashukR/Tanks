using Godot;
using System;

public partial class Town1 : Town
{
    private bool is_ai;
    public TextureButton on_ai;
    private Area2D bullet_area;
    private bool flag_area;
    private BoxContainer info;
    private bool this_is_pick_unit;
    public override void _Ready()
    {
        info = GetNode<BoxContainer>("%info");
        bullet_area = GetNode<Area2D>("%bullet_area");
        on_ai = GetNode<TextureButton>("%on_ai_town");
        GlobalManager.Instance.pick_unit += pick_unit;
        bullet_area.BodyEntered += (body) => Entered(body);
        on_ai.Pressed += () => 
        {
            is_ai = !is_ai;
            if(is_ai)info.Hide();
            info.Show();
            this_is_pick_unit = true;
            GlobalManager.Instance.EmitSignal("pick_unit");
            flag_area = true;
        };
        base._Ready();
        
    }
    private void pick_unit()
    {
        if(!is_ai && !this_is_pick_unit)is_ai = true;
        if(is_ai && !info.IsQueuedForDeletion())info.Hide();
        this_is_pick_unit = false;
    }
    public async void  Entered(Node2D body)
    {
        if(body is Bullet bullet)
        {
            if(patron > 0 && can_shoot)
            {
                Vector2 future_pos = bullet.GlobalPosition + bullet.dir * bullet.speed * 0.03f;
                var tween = CreateTween();
                tween.SetEase(Tween.EaseType.InOut);
                tween.SetTrans(Tween.TransitionType.Sine);
                tween.TweenProperty(pushka, "rotation", (GlobalPosition - future_pos).Normalized().Angle(), 0.2f);
                await ToSignal(tween, "finished");
                GlobalManager.Instance.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false, false, pushka.Rotation, new Vector2(0.165f, 0.171f), 50);
                can_shoot = false;
                t.Start();
                patron--;
                patron_l.Text = $"Town patron: {patron}";
            }
        }
    }
    static public void set_shader(Node parent, bool _render, string name_param)
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
    public override void _Input(InputEvent @event)
    {
        if(!is_ai)base._Input(@event);
    }
    public override  void  _Process(double delta)
    {
        if(!is_ai)
        {
            base._Process(delta);
        }
        else if(flag_area)
        {
            flag_area = false;
            bullet_area.Monitoring = true;
            
        }
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GlobalManager.Instance.pick_unit -= pick_unit;
        on_ai.QueueFree();
    }
}
