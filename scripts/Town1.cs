using Godot;
using System;

public partial class Town1 : Town, ITown
{
    private bool _is_ai{set;get;}
    public TextureButton on_ai;
    private Area2D bullet_area, unit_detected;
    [Export] public float bullet_area_koef { get; set; } = 0.08f;
    [Export] public float time_tween { get; set; } = 0.08f;
    [Export] public Vector2 bullet_size { get; set; } = new Vector2(0.165f, 0.171f);

    [Export] public int bullet_damage { get; set; } = 50;


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
            info.Visible = !_is_ai;
        }
    }
    public override void _Ready()
    {
        info = GetNode<BoxContainer>("%info");
        bullet_area = GetNode<Area2D>("%bullet_area");
        unit_detected = GetNode<Area2D>("%unit_area");
        on_ai = GetNode<TextureButton>("%on_ai_town");
        GlobalManager.Instance.pick_unit += pick_unit;
        bullet_area.BodyEntered += (body) =>{
            GamaUtilits.EnteredBulletInTownZone(body, this, true);
            upd_h();
        };
        on_ai.Pressed += () => 
        {
            is_ai = !is_ai;
            this_is_pick_unit = true;
            GlobalManager.Instance.EmitSignal("pick_unit");
            flag_area = true;
        };
        unit_detected.BodyEntered += (body) =>{
            GamaUtilits.EnteredBulletInTownZone(body, this, false);
            upd_h();
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
