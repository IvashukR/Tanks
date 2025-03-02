using Godot;
using System;

public partial class SettingWindow : Control
{
    private OptionButton resolution_w;
    private CheckBox fps;
    private Button save_btn;
    private readonly string path_cfg = "user://save_data.cfg";
    public override void _Ready()
    {
        fps = GetNode<CheckBox>("%fps");
        resolution_w = GetNode<OptionButton>("%resolution_w");
        resolution_w.ItemSelected += ClickWindowSizes;
        fps.Toggled += CheckedFps;
        save_btn = GetNode<Button>("%save_btn");
        save_btn.Pressed += SaveCfg;
        LoadCfg();
    }
    private void ClickWindowSizes(long index)
    {
        if(resolution_w.GetItemText((int)index) == "Full Screen")
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
            return;
        }
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        string s_index = resolution_w.GetItemText((int)index);
        var s_size = s_index.Split("x");
        long width = long.Parse(s_size[0]);
        long height = long.Parse(s_size[1]);
        DisplayServer.WindowSetSize(new Vector2I((int)width, (int)height));

    }
    private void SaveCfg()
    {
        ConfigFile cfg = new ConfigFile();
        if(AudioMeneger.ExistsFile(path_cfg))cfg.Load(path_cfg);
        cfg.SetValue("Other Setting", "Window Size", resolution_w.GetItemText(resolution_w.Selected));
        cfg.Save(path_cfg);
    }
    private void LoadCfg()
    {
        ConfigFile cfg = new ConfigFile();
        if(cfg.Load(path_cfg) != Error.Ok)return;
        string text = (string)cfg.GetValue("Other Setting", "Window Size");
        resolution_w.Select(GetIndexByText(text));
        ClickWindowSizes(resolution_w.GetSelectedId());

    }
    private int GetIndexByText(string text)
    {
        for(int i = 0; i < resolution_w.ItemCount; i++)
        {
            if(resolution_w.GetItemText(i) == text)
            return i;
        }
        return 0;
    }
    private void CheckedFps(bool check)
    {
        GlobalManager.Instance.EmitSignal("fps", check);
    }
}
