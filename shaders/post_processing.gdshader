shader_type canvas_item;

uniform vec2 size = vec2(32.0, 28.0);
// If you intend to read from mipmaps with `textureLod()` LOD values greater than `0.0`,
// use `filter_nearest_mipmap` instead. This shader doesn't require it.
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
		vec2 uvRel = vec2(UV.x - 0.5, UV.y - 0.5);
		float r = sqrt(pow(uvRel.x, 2.0) + pow(uvRel.y, 2.0));
		float theta = atan(uvRel.y / uvRel.x);
		if(uvRel.x < 0.0) {
			if(uvRel.y > 0.0) {
				theta += PI/2.0;
			} else {
				theta -= PI/2.0;
			}
		}
		vec2 polar = vec2(r, theta);
		vec2 backToUv = vec2(r*cos(theta), r*sin(theta));

        COLOR = textureLod(screen_texture, backToUv, 0.0);
}