using Godot;
using System;
using TanksUtilits;

namespace Player.Transport;

public partial class Town : CharacterBody2D
{
	public  Sprite2D pushka { get; set; }
    public  bool can_shoot { get; set; } = true;
    public Marker2D marker { get; set; }
    public Timer t { get; set; }
    private Label patron_l;
    [Export] public int patron  { get; set; } = 3;
    public int proch = 75;
    private Label hp_l;
    private ShaderMaterial sm;
    private CpuParticles2D blam_particles;
    
    protected void upd_h()
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
    protected void upd_patron_l() => patron_l.Text = $"Town patron: {patron}";
	public override void _Ready()
    {
        blam_particles = GetNode<CpuParticles2D>("%blam");
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
        GlobalManager.Instance.destroyed_town += destroy;
    }
    private void destroy(Node2D node)
    {
        if(node == this)
		{
			GamaUtilits.DestroyTown(proch,blam_particles, this, sm);
			if(proch <= 0)GlobalManager.Instance.EmitSignal("fail");
		}
    }
    public override void _Process(double delta)
    {
		if (can_shoot && patron != 0)
        {
           pushka.RotationDegrees += 0.9f;
        }
        
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if(GlobalManager.Instance.block_input)return;
		if (@event is InputEventMouseButton mouseEvent )
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot)
            {
                Shoot();
            }
        }
        
    }
    protected void Shoot()
    {
        if (patron <= 0)
        {
            can_shoot = false;
            return;
        }
		GamaUtilits.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false, pushka.Rotation, new Vector2(0.165f, 0.171f), 50, -1);
        can_shoot = false;
        t.Start();
        patron--;
        patron_l.Text = $"Town patron: {patron}";
    }
    public override void _ExitTree()
	{
		GlobalManager.Instance.del_t -= upd_h;
        GlobalManager.Instance.destroyed_town -= destroy;
	}


}