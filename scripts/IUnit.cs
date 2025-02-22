using Godot;
//Interfaces for take damage units
public interface IUnit
{
    public FSM fsm { get; set; }
    public int proch { get; set; }
    public Label hp_l { get; set; }
    public Node2D voin_sprite { get; set; }
    public string name_unit { get; set; }

}