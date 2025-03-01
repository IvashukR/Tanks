using Godot;
using System;

public partial class AudioMeneger : Control
{
    private HSlider sfx_s, music_s, master_s;
    private readonly string path_cfg = "user://save_data.cfg";
    private Button save_btn;
    public override void _Ready()
    {
        save_btn = GetNode<Button>("%save_btn");
        sfx_s = GetNode<HSlider>("%sfx_s");
        music_s = GetNode<HSlider>("%music_s");
        master_s = GetNode<HSlider>("%master_s");
        SetValueSlider();
        master_s.ValueChanged += (value) => SetDb(value, 0);
        sfx_s.ValueChanged += (value) => SetDb(value, 1);
        music_s.ValueChanged += (value) => SetDb(value, 2);
        save_btn.Pressed += SaveCfg;
        LoadCfg();
    }
    private void SetValueSlider()
    {
        master_s.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0));
        sfx_s.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(1));
        music_s.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(2));
    }
    private void SetDb(double value , int index_bus)
    {
        AudioServer.SetBusVolumeDb(index_bus, Mathf.LinearToDb((float)value));
    }
    private void SaveCfg()
    {
        ConfigFile cfg = new ConfigFile();
        cfg.SetValue("AudioSlider", "Master", master_s.Value);
        cfg.SetValue("AudioSlider", "SFX", sfx_s.Value);
        cfg.SetValue("AudioSlider", "Music", music_s.Value);
        Error error = cfg.Save(path_cfg);
        if(error != Error.Ok)throw new Exception("Dont Save File!");
    }
    private void LoadCfg()
    {
        ConfigFile cfg = new ConfigFile();
        if (cfg.Load(path_cfg) == Error.Ok)
        {
            sfx_s.Value = (double)cfg.GetValue("AudioSlider", "SFX");
            master_s.Value = (double)cfg.GetValue("AudioSlider", "Master");
            music_s.Value = (double)cfg.GetValue("AudioSlider", "Music");
            SetDb(sfx_s.Value, 1);
            SetDb(master_s.Value, 0);
            SetDb(music_s.Value, 2);
        }
        else GD.Print("NOT LOAD CFG");
    }
}
