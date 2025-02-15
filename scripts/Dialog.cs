using Godot;
using System;
using System.Security.AccessControl;
using System.Threading.Tasks;

public partial class Dialog : MarginContainer
{
	private Timer t;
	public Label label;
	private int letter_index = 0;
	public const int max_widht = 220;
	private float punctuation_time = 0.2f;
	public AnimatedSprite2D pig;
	public TextureButton skip;
	private float space_time = 0.06f;
	private float letter_time = 0.03f;
	private TaskCompletionSource<object> _resizeTcs; 
	public string text = " ";
	
	public override  void _Ready()
	{
		pig = GetNode<AnimatedSprite2D>("%pig");
		pig.Play();
		skip = GetNode<TextureButton>("%skip");
		skip.Pressed += () => {
			GlobalManager.Instance.EmitSignal("skip_d");
			QueueFree();
		};
		t = GetNode<Timer>("%t");
		t.Timeout += to;
		label = GetNode<Label>("%txt");
		Resized += resize;
		t.Start();
		GlobalManager.Instance.finish_d += pig_stop;


	}
	private void to() => display_letter();
	public  async void display_text(string text_display)
	{
		text = text_display;
		if (Size.X > max_widht)
		{
			label.AutowrapMode = TextServer.AutowrapMode.Word;
			await wR(); //for x
			await wR(); //for y
		}
		label.Text = "";
		display_letter();
	}
	private void pig_stop() => pig.Stop();
	public void display_letter()
	{
		if(letter_index > text.Length - 1)
		{
			GlobalManager.Instance.EmitSignal("finish_d");
			return;
		}
		label.Text += text[letter_index];
		letter_index++;
		if(letter_index > text.Length - 1)
		{
			GlobalManager.Instance.EmitSignal("finish_d");
			return;
		}
		switch (text[letter_index])
		{
			case '.':
			case ',':
			case '!':
			case '?':
			t.Start(punctuation_time);
			break;
			case ' ':
			t.Start(space_time);
			break;
			default:
			t.Start(letter_time);
			break;
		} 
	}
	private async Task wR()
	{
		 _resizeTcs = new TaskCompletionSource<object>();
		 await _resizeTcs.Task;
	}
	private void resize() => _resizeTcs?.SetResult(null);
	public override void _ExitTree()
	{
		GlobalManager.Instance.finish_d -= pig_stop;
	}

	
	
}
