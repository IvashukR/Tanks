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
	private Control control;
	private bool w = false;
	[Signal]
	public delegate void StartEventHandler();
	private TextureButton restart;
	private Phone phone;
	private Label fps_l;
	[Export] protected string inp = "Привет друг, вижу по твоему личному делу что у тебя не нету никакого оптита в военом деле но парень смишленний. Как ты знаеш  у нас тут война с коровами за ресурси, управляй войсками чтоб уничтожить вражескую станцию ";
	protected PackedScene s  = ResourceLoader.Load<PackedScene>("res://scene/trenirovka.tscn");
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
		GetTree().Paused = true;
		phone = GetNode<Phone>("%Phone");
		fps_l = GetNode<Label>("%fps_l");
		phone.Show();
		phone.anim_phone.Play("open");
		phone.anim_phone.AnimationFinished += (animationName) => phone.Hide();
		GlobalManager.Instance.fail += losse;
		GlobalManager.Instance.win += win;
		restart = GetNode<TextureButton>("%restart");
		restart.Pressed += () => {
			//GetTree().ChangeSceneToPacked(s);
			GetTree().ReloadCurrentScene();
		};
		info = GetNode<BoxContainer>("%info");
		enemy = GetNode<StaticBody2D>("%town_enemy");
		blast = GetNode<CpuParticles2D>("%blast");
		sh = blast.Material as ShaderMaterial;
		go_s = GetNode<Sprite2D>("%go");
		GlobalManager.Instance._fps += fps;
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
		dialog.display_text(inp);
		fps_l.Visible = GlobalManager.Instance.fps;
	}
	private void fps(bool value) => fps_l.Visible = value;
	protected void losse() => restart.Visible = true;
	protected void win()
	{

	}
	
	public override async void _Process(double delta)
	{
		if (go_s.Texture == go[2] && a)
		{
			a = false;
			timer.Stop();
			await ToSignal(GetTree().CreateTimer(1.3f), "timeout");
			GetTree().Paused = false;
			go_s.Visible = false;
			info.Visible = true;
			EmitSignal("Start");
			
		}
        if(GlobalManager.Instance.fps)fps_l.Text = $"FPS: {Engine.GetFramesPerSecond()}";
    

	}
	public override void _ExitTree()
	{
		GlobalManager.Instance.skip_d -= skip;
		GlobalManager.Instance.fail -= losse;
		GlobalManager.Instance.win -= win;
		GlobalManager.Instance._fps -= fps;
	}
}
