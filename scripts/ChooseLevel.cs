using Godot;
using System;

public partial class ChooseLevel : Control
{
    [Export] private int n_columns;
    [Export] private int max_level;
    private GridContainer container_levels;
    private Texture2D texture_btn_level = (Texture2D)ResourceLoader.Load("res://textures/levels_btn.png");
    private Texture2D texture_btn_level_locked = (Texture2D)ResourceLoader.Load("res://textures/levels_btn_locked.png");
    public override void _Ready()
    {
        container_levels = GetNode<GridContainer>("%container_level");
        container_levels.Columns = n_columns;
        for(int i = 0; i < max_level; i++)
        {
            var btn_level = new TextureButton();
            var level_l = new Label();
            level_l.Text = Convert.ToString(i);
            if(i == 1 || GlobalManager.Instance.GameLevels.Count <= i)btn_level.TextureNormal = texture_btn_level;
            else btn_level.TextureNormal = texture_btn_level_locked;
            container_levels.AddChild(btn_level);
            btn_level.AddChild(level_l);
            btn_level.Pressed += () =>
            {
                if(btn_level.TextureNormal == texture_btn_level)
                GetTree().ChangeSceneToPacked(GlobalManager.Instance.GameLevels[i]);
            };
            btn_level.TreeExited += () => btn_level = null;
        
        }
    }
}
