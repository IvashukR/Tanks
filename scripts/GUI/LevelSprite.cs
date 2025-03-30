using Godot;
using System;


public partial class LevelSprite : Node2D
{
    [Export] int level;
    private Label n_lvl_l, n_lvl_l_2;
    private AnimationPlayer anim;
    private Polygon2D galka, red_osnova, gray_osnova;
    public override void _Ready()
    {
        n_lvl_l = GetNode<Label>("Polygon2D16/Label");
        n_lvl_l_2 = GetNode<Label>("Polygon2D17/Label");
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        galka = GetNode<Polygon2D>("Polygon2D19");
        red_osnova = GetNode<Polygon2D>("Polygon2D16");
        gray_osnova = GetNode<Polygon2D>("Polygon2D17");
        n_lvl_l.Text = (level + 1).ToString();
        n_lvl_l_2.Text = n_lvl_l.Text;
        if(GlobalManager.Instance.last_level == level)
        anim.Play("a");
        else if(GlobalManager.Instance.last_level > level)
        galka.Show();
        else 
        {
            red_osnova.Hide();
            gray_osnova.Show();
        }
    }
}
