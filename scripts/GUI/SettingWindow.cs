using Godot;
using System;

public partial class SettingWindow : Control
{
    private OptionButton resolution_w;
    private readonly string path_cfg = "user://save_data.cfg";
    public override void _Ready()
    {
        resolution_w = GetNode<OptionButton>("%resolution_w");
        resolution_w.ItemSelected += (index) => ClickWindowSizes(index);
    }
    private void ClickWindowSizes(long index)
    {
        if(resolution_w.GetItemText((int)index) == "Full Screen")
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
            return;
        }
        string s_index = resolution_w.GetItemText((int)index);
        var s_size = s_index.Split("x");
        long width = long.Parse(s_size[0]);
        long height = long.Parse(s_size[1]);
        DisplayServer.WindowSetSize(new Vector2I((int)width, (int)height));

    }
}
