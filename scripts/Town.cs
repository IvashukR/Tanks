using Godot;
using System;

public partial class Town : CharacterBody2D
{
	public Sprite2D pushka;
    public bool can_shoot = true;
    public Marker2D marker;
    protected Timer t;
    public Node parent;
    protected Label patron_l;
    protected int patron = 3;
    public int proch = 75;
    private Label hp_l;
    public bool is_boom;
    private ShaderMaterial sm;
    private CpuParticles2D blam_particles;
    
    private void upd_h()
    {
        if(hp_l == null )return;
        if (proch >= 0)
        {
            hp_l.Text = $"Town health: {proch}";
        }
        else
        {
            hp_l.Text = "Town health: 0";
        }
    }
    private async void Destroy()
    {
        if(proch <= 0)
        {
            if(is_boom)return;
            is_boom = true;
            blam_particles.Emitting = true;
            for (float i = 0.0f; i <= 1; i += 0.3f)
            {
			    await ToSignal(GetTree().CreateTimer(0.19f), "timeout");
                sm.SetShaderParameter("glow_strength", i);
            }
            QueueFree();
        }
        else
        {
            Town1.set_shader(this, true, "damage");
            await ToSignal(GetTree().CreateTimer(0.9f), "timeout");
            Town1.set_shader(this, false, "damage");
        }

        
    }
	public override void _Ready()
    {
        blam_particles = GetNode<CpuParticles2D>("%blam");
        parent = GetParent();
        patron_l = GetNode<Label>("%patron_l");
        patron_l.Text = $"Town patron: {patron}";
        hp_l = GetNode<Label>("%hp_l");
        GlobalManager.Instance.del_t += upd_h;
        hp_l.Text = $"Town health: {proch}";
        pushka = GetNode<Sprite2D>("%pushka");
		marker = GetNode<Marker2D>("%marker");
        t = new Timer();
        t.WaitTime = 0.7f;
        t.OneShot = true;
        AddChild(t);
        sm = blam_particles.Material as ShaderMaterial;
        t.Timeout += () => can_shoot = true;
        GlobalManager.Instance.destroyed_town += Destroy;
    }
    public override void _Process(double delta)
    {
		if (can_shoot && patron != 0)
        {
           pushka.RotationDegrees += 0.9f;
        }
        if (patron == 0 && GetNodeOrNull("%town_enemy") != null && GetTree().GetNodesInGroup("bullet").Count == 0)
        {
            GlobalManager.Instance.EmitSignal("fail");
        }
        
    }
    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent )
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot )
            {
                if(parent is Level1 level)
                {
                    foreach(TextureButton btn in level.all_btn_ui)
                    {
                        if(btn.GetGlobalRect().HasPoint(mouseEvent.Position) && btn.Visible == true)return;
                    }
                }
                if (patron <= 0)
                {
                    can_shoot = false;
                    return;
                }
				GlobalManager.Instance.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false, false, pushka.Rotation, new Vector2(0.165f, 0.171f), 50);
                can_shoot = false;
                t.Start();
                patron--;
                patron_l.Text = $"Town patron: {patron}";
            }
        }
        
    }
    public override void _ExitTree()
	{
		GlobalManager.Instance.del_t -= upd_h;
        GlobalManager.Instance.destroyed_town -= Destroy;
	}


}