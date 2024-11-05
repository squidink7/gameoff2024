shader_type canvas_item;

uniform sampler2D occluder_texture;
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
	COLOR = texture(screen_texture, SCREEN_UV);
	COLOR.a = COLOR.a * texture(occluder_texture, SCREEN_UV).a;
}
