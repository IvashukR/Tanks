using Godot;
using System;

public partial class Town : CharacterBody2D
{
	public Sprite2D pushka;
    public bool can_shoot = true;
    public Marker2D marker;
    private Timer t;
    private Label patron_l;
    protected int patron = 3;
    public int proch = 75;
    private Label hp_l;
    private void upd_h()
    {
        if (proch >= 0)
                {
                    hp_l.Text = $"Town health: {proch}";
                }
                else
                {
                    hp_l.Text = "Town health: 0";
                }
    }
	public override void _Ready()
    {
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
        t.Timeout += () => can_shoot = true;
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
		if (@event is InputEventMouseButton mouseEvent)
        {
            
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && can_shoot)
            {
                if (patron <= 0)
                {
                    can_shoot = false;
                    return;
                }
				GlobalManager.Instance.shoot(pushka.GlobalPosition, marker.GlobalPosition, this, false, false, pushka.Rotation, new Vector2(0.165f, 0.171f), 7);
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
	}


}