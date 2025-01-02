using Godot;
using System;
using System.Timers;

public partial class Phone : ColorRect
{
	private bool life = true;
	public float elaps_time = 0.0f;
	[Export] public float duration_sh = 3.5f;
	private ShaderMaterial sh;
	public int a;
	public void set_a(out int a, int val)
	{
		a = val ;
	}
	public override void _Ready()
	{
		sh = Material as ShaderMaterial;
		
		
	}

	
	public override void _Process(double delta)
	{
	if (a == 0)
	{
		float circ_val = 0.0f;
		if (elaps_time <= duration_sh)
		{
			elaps_time += (float)delta;
			var t = elaps_time / duration_sh;
			circ_val = Mathf.Lerp(0.0f, 1.0f, t);
		}
		else if(elaps_time > duration_sh && life)
		{
			circ_val = 1.0f;
			Visible = false;
			life = false;
		}
		sh.SetShaderParameter("radius", circ_val);
	}
	else
	{
		float circ_val = 1.0f;
		if (elaps_time <= duration_sh)
		{
			elaps_time += (float)delta;
			var t = elaps_time / duration_sh;
			circ_val = Mathf.Lerp(1.0f, 0.0f, t);
		}
		else if(elaps_time > duration_sh && life)
		{
			circ_val = 0.0f;
			Visible = false;
			life = false;
		}
		sh.SetShaderParameter("radius", circ_val);
	}
		
	}
	
}

