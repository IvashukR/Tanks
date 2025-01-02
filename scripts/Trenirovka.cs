using Godot;
using System;
public partial class Trenirovka : Node
{
	
	public MarginContainer d;
	private Sprite2D go_s;
	private int id = 0;
	private Timer timer;
	private bool a = true;
	public Sprite2D boom;
	public CpuParticles2D blast;
	public StaticBody2D enemy;
	private ShaderMaterial sh;
	private BoxContainer info;
	private bool flag = false;
	private bool w = false;
	private TextureButton restart;
	private PackedScene s = ResourceLoader.Load<PackedScene>("res://scene/trenirovka.tscn");
	
	private Texture2D[] go = {
		(Texture2D)ResourceLoader.Load("res://textures/two.png"),
		(Texture2D)ResourceLoader.Load("res://textures/three.png"),
		(Texture2D)ResourceLoader.Load("res://textures/GO!.png")
	};
	private void skip()
	{
		flag = true;
		go_s.Visible = true;
		timer.Start();
	}
	public override  void _Ready()
	{
		GlobalManager.Instance.fail += losse;
		restart = GetNode<TextureButton>("%restart");
		restart.Pressed += () => {
			GetTree().ChangeSceneToPacked(s);
		};
		info = GetNode<BoxContainer>("%info");
		enemy = GetNode<StaticBody2D>("%town_enemy");
		blast = GetNode<CpuParticles2D>("%blast");
		sh = blast.Material as ShaderMaterial;
		GetTree().Paused = true;
		go_s = GetNode<Sprite2D>("%go");
		timer = new Timer();
		timer.WaitTime = 1;
		
		AddChild(timer);
		timer.Timeout += () => {
			
			go_s.Texture = go[id];
			id++;
			
		};

		d = GetNode<MarginContainer>("%Dialog");
		Dialog dialog = (Dialog) d;
		GlobalManager.Instance.skip_d += skip;
		string inp = "Привет друг, вижу по твоему личному делу что у тебя не нету никакого оптита в военом деле но парень смишленний. Как ты знаеш  у нас тут война с коровами за ресурси, управляй войсками чтоб уничтожить вражескую станцию ";
		dialog.display_text(inp);
		

		
	}
	protected void losse() => restart.Visible = true;
	protected void win()
	{

	}
	
	public override async void _Process(double delta)
	{
		if (go_s.Texture == go[2] && a)
		{
			timer.Stop();
			await ToSignal(GetTree().CreateTimer(1.3f), "timeout");
			GetTree().Paused = false;
			go_s.Visible = false;
			info.Visible = true;

			a = false;
			
		}
		if (GetNodeOrNull("%town") == null && GetTree().GetNodesInGroup("bullet").Count == 0 && flag )
		{
			losse();
			flag = false;

		}
		if (GetNodeOrNull("%town_enemy") == null && !w)
		{
			win();
			w = true;
		}


	}
	public override void _ExitTree()
	{
		GlobalManager.Instance.skip_d -= skip;
	}
}
