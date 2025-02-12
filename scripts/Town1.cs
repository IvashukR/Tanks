using Godot;
using System;

public partial class Town1 : Town, ITown
{
    private bool _is_ai{set;get;}
    public TextureButton on_ai;
    private Area2D bullet_area;
    [Export] public float bullet_area_koef { get; set; } = 0.3f;
    public float time_tween { get; set; } = 0.2f;
    public Vector2 bullet_size { get; set; } = new Vector2(0.165f, 0.171f);

    public int bullet_damage { get; set; } = 50;


    private bool flag_area = true;
    private BoxContainer info;
    private bool this_is_pick_unit;
    private bool is_ai
    {
        get => _is_ai;
        set
        {
            _is_ai = value;
            bullet_area.Monitoring = _is_ai;
        }
    }
    public override void _Ready()
    {
        info = GetNode<BoxContainer>("%info");
        bullet_area = GetNode<Area2D>("%bullet_area");
        on_ai = GetNode<TextureButton>("%on_ai_town");
        GlobalManager.Instance.pick_unit += pick_unit;
        bullet_area.BodyEntered += (body) => Entered(body, this);
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
        GlobalManager.Instance.card_click += ClickCard;
        
    }
    private void ClickCard(){if(!is_ai)is_ai = true;}
    private void pick_unit()
    {
        if(!is_ai && !this_is_pick_unit)is_ai = true;
        if(is_ai && !info.IsQueuedForDeletion())info.Hide();
        this_is_pick_unit = false;
    }
    public static async void  Entered(Node2D body, Node2D obj)
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
                        GlobalManager.Instance.shoot(town.pushka.GlobalPosition, town.marker.GlobalPosition, obj, false, false, town.pushka.Rotation, town.bullet_size, town.bullet_damage);
                        town.can_shoot = false;
                        town.t.Start();
                        town.patron--;
                        town.patron_l.Text = $"Town patron: {town.patron}";
                    }
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
        
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GlobalManager.Instance.card_click -= ClickCard;
        GlobalManager.Instance.pick_unit -= pick_unit;
        if(GetParent() is Level1 level)level.all_btn_ui.Remove(on_ai);
        on_ai.QueueFree();
    }
}
