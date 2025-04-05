extends Sprite2D
@onready var audio_btn = %audio_btn
@onready var audio_anim = $"../AnimationPlayer"
var music_bus = AudioServer.get_bus_index("Music")
var sfx_bus   = AudioServer.get_bus_index("SFX")
var master_bus = AudioServer.get_bus_index("Master")
func _set_visible() -> void:
	visible = !visible

func _ready() -> void:
	if(AudioServer.is_bus_mute(music_bus) and AudioServer.is_bus_mute(sfx_bus) and AudioServer.is_bus_mute(master_bus)):
		show()
func mute_bus(mute: bool):
	AudioServer.set_bus_mute(music_bus, mute)
	AudioServer.set_bus_mute(sfx_bus, mute)
	AudioServer.set_bus_mute(master_bus, mute)
func _on_audio_btn_pressed() -> void:
	if not audio_anim.is_playing():
		audio_anim.play("start")
	await  audio_anim.animation_finished
	mute_bus(visible)
