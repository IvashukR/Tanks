using Godot;
using System;
using System.Threading.Tasks;

public partial class Well : StaticBody2D
{
    public float low;
	private int _proch = 75;
    private AnimatedSprite2D well_t;
    private bool crash;
	public int Proch
    
    
    {
        get { return _proch; }
        set
        {
            _proch = value;
            if (_proch <= 0)
            {
                _ = kill();
            }
            else if (_proch <= low && !crash)
            {
                crash = true;
                well_t?.Play();
            }
        }
    }
	
	public override void _Ready()
	{
        low = _proch * 50 / 100;
        well_t = GetNode<AnimatedSprite2D>("%well_t");
		
	}
    public  async Task  kill()
	{
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(this, "modulate:a", 0f, 1.5f);
        await ToSignal(tween, "finished");
        QueueFree();
		
	}
	
}
