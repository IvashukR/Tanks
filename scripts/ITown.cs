using Godot;
public interface ITown
{
    public int patron { get; set; }
	public bool can_shoot { get; set; }
    [Export] public float bullet_area_koef { get; set; }
    public float time_tween { get; set; }
    public Timer t { get; set; }
    public Sprite2D pushka { get; set; }
    public Marker2D marker { get; set; }
    public Vector2 bullet_size { get; set; }
    public int bullet_damage { get; set; }


}