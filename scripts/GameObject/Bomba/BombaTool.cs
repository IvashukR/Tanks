using Godot;
using System;

namespace GameObjects;
[Tool]
public partial class BombaTool : Node2D
{
    private float _progress {set;get;} = 21.5f;
    private PathFollow2D path_move;
    [Export] 
    public float progress 
    {
        get => _progress;
        set
        {
            _progress = value;
            UpdateProgress();
        }
    }
    public override void _Ready()
    {
        path_move = GetNode<PathFollow2D>("%path_move");
        
    }

    private void UpdateProgress()
    {
        path_move.Progress = _progress;
    }
}
