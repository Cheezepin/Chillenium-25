shader_type canvas_item;

uniform bool screen_color_bw = false;
uniform bool screen_color_other_bw = false;
uniform vec4 screen_color 		: source_color = vec4(1.0);
uniform vec4 screen_color_other : source_color = vec4(1.0);
uniform float progress = 0;
// If you intend to read from mipmaps with `textureLod()` LOD values greater than `0.0`,
// use `filter_nearest_mipmap` instead. This shader doesn't require it.
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
	vec2 uv = UV;
	vec4 color = (UV.x > progress) ? screen_color : screen_color_other;
	vec4 bw = vec4(0.2126, 0.7152, 0.0722, 1.0);
	vec4 pixel = textureLod(screen_texture, uv, 0);
	//if((UV.x < progress && screen_color_other_bw) || (UV.x >= progress && screen_color_bw)) {
		float avg = (pixel.x + pixel.y + pixel.z) / 3.0;
		COLOR = vec4(avg, avg, avg, 1.0);
	//} else {
		//COLOR = pixel*color;
	//}
	if(!((UV.x < progress && screen_color_other_bw) || (UV.x >= progress && screen_color_bw))) { COLOR *= color; }
}