using Godot;
using GameObjects;

namespace Player.Transport;

public partial class Town : StaticBody2D
{
    [Export] public TowerLogicShoot logic  {set;get;}
    private Label patron_l;
    
    private Label hp_l;
    
    protected void upd_h()
    {
        if(hp_l == null )return;
        if (logic.proch >= 0)
        {
            hp_l.Text = $"Town health: {logic.proch}";
        }
        else
        {
            hp_l.Text = "Town health: 0";
        }
        
    }
    protected void upd_patron_l() => patron_l.Text = $"Town patron: {logic.patron}";
	public override void _Ready()
    {
        patron_l = GetNode<Label>("%patron_l");
        patron_l.Text = $"Town patron: {logic.patron}";
        hp_l = GetNode<Label>("%hp_l");
        hp_l.Text = $"Town health: {logic.proch}";
    }
    public override void _Process(double delta)
    {
		if (logic.can_shoot && logic.patron != 0)
        {
           logic.pushka.RotationDegrees += 0.9f;
        }
        
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if(GlobalManager.Instance.block_input)return;
		if (@event is InputEventMouseButton mouseEvent )
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed && logic.can_shoot)
            {
                Shoot();
            }
        }
        
    }
    public virtual void Shoot()
    {
        logic.Shoot();
        upd_patron_l();
    }
    
    public virtual void TakeDamage(int damage)
    {
        logic.TakeDamage(damage);
        upd_h();
    }
    


}