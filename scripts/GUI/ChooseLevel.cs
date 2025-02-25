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
        SetLevelsGrid();
    }
    private void SetLevelsGrid()
    {
        for(int i = 0; i < max_level; i++)
        {
            var btn_level = new TextureButton();
            var level_l = new Label();
            level_l.Text = Convert.ToString(i + 1);
            if(i == 0 || GlobalManager.Instance.GameLevels.Count >= i + 1)btn_level.TextureNormal = texture_btn_level;
            else btn_level.TextureNormal = texture_btn_level_locked;
            container_levels.AddChild(btn_level);
            level_l.Scale = btn_level.Scale * 5;
            level_l.GlobalPosition = new Vector2((btn_level.TextureNormal.GetSize().X / 2) - (btn_level.TextureNormal.GetSize().X * 2 / 100), (btn_level.TextureNormal.GetSize().Y / 2) - (btn_level.TextureNormal.GetSize().Y * 18 / 100));
            btn_level.AddChild(level_l);
            btn_level.Pressed += () =>
            {
                if(btn_level.TextureNormal == texture_btn_level)
                {
                    GetTree().ChangeSceneToPacked(GlobalManager.Instance.GameLevels[Convert.ToInt32(btn_level.GetChild<Label>(0).Text) - 1]);
                }
            };
            btn_level.TreeExited += () => btn_level = null;
        }
    }
}
