using Godot;
using System;

namespace GameView;
public partial class SettingWindow : Control
{
    private OptionButton resolution_w;
    private CheckBox fps;
    private Button def_cfg_btn;
    private AudioMeneger audio;
    private readonly string path_cfg = "user://save_data.cfg";
    private readonly string def_path_cfg = "res://default_setting.cfg";
    public override void _Ready()
    {
        audio = GetNode<AudioMeneger>("%audio_m");
        fps = GetNode<CheckBox>("%fps");
        resolution_w = GetNode<OptionButton>("%resolution_w");
        resolution_w.ItemSelected += ClickWindowSizes;
        fps.Toggled += CheckedFps;
        def_cfg_btn = GetNode<Button>("%def_cfg_btn");
        def_cfg_btn.Pressed += LoadDefCfg;
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
        cfg.SetValue("Other Setting", "FPS", fps.ButtonPressed);
        cfg.Save(path_cfg);
    }
    private void LoadCfg()
    {
        ConfigFile cfg = new ConfigFile();
        if(cfg.Load(path_cfg) != Error.Ok)return;
        string text = (string)cfg.GetValue("Other Setting", "Window Size");
        resolution_w.Select(GetIndexByText(text));
        fps.ButtonPressed = (bool)cfg.GetValue("Other Setting", "FPS");
        CheckedFps(fps.ButtonPressed);
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
        GlobalManager.Instance.fps = check;
        GlobalManager.Instance.EmitSignal("_fps", check);
    }
    private void LoadDefCfg()
    {
        ConfigFile src_cfg = new ConfigFile();
        if(src_cfg.Load(def_path_cfg) != Error.Ok)return;
        ConfigFile target_cfg = new ConfigFile();
        foreach (string section in src_cfg.GetSections())
        {
            foreach (string key in src_cfg.GetSectionKeys(section))
            {
                target_cfg.SetValue(section, key, src_cfg.GetValue(section, key));
            }
        }
        target_cfg.Save(path_cfg);
        audio.LoadCfg();
        LoadCfg();
    }
    public override void _ExitTree()
    {
        SaveCfg();
    }
}
