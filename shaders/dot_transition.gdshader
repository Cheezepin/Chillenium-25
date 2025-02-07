shader_type canvas_item;

uniform float radius = 0.1;
uniform float circlesX = 4;
uniform float circlesY = 4;

void vertex() {
	// Called for every vertex the material is visible on.
	COLOR = vec4(0, 0, 0, 0);
}

bool colorCircle(vec2 uv, float cX, float cY) {
	float dist = (pow(uv.x - cX, 2) + pow(uv.y - cY, 2));
	if(dist > 0.0) {
		float rad = radius;
		if(rad > .2) {rad = .4 - radius; cX = 1.0 - cX; cY = 1.0 - cY;}
		rad += - (cX/20.0) - (cY/20.0); //controls the wave across
		if(rad < 0.0) {return false;}
		if(dist < pow(rad,2.0)) {
			return true;
		}
	}
	return false;
}

void fragment() {
	// Called for every pixel the material is visible on.
	//COLOR = vec4(0, 0, 0, 0);
	
	float invCX = 1.0 / circlesX;
	float invCY = 1.0 / circlesY;
	float cY = floor(UV.y * circlesY) / circlesY + (0.5*invCY);
	float xOffset = 0.0;
	if(int(UV.y * circlesY) % 2 == 0) {xOffset = (0.5*invCX);}
	float cX = floor((UV.x - xOffset) * circlesX) / circlesX + (0.5*invCX) + xOffset;
	vec2 uv = UV;
	
	if(colorCircle(uv, cX, cY)
	|| colorCircle(uv, cX - (0.5*invCX), cY - (invCY))
	|| colorCircle(uv, cX + (0.5*invCX), cY - (invCY))
	|| colorCircle(uv, cX - (0.5*invCX), cY + (invCY))
	|| colorCircle(uv, cX + (0.5*invCX), cY + (invCY))
	|| colorCircle(uv, cX - (invCX), cY)
	|| colorCircle(uv, cX + (invCX), cY)) {
		COLOR = vec4(0,0,0,1);
	}
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
