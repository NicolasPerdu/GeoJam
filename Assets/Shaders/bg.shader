Shader "GLSL basic shader" { // defines the name of the shader 
	SubShader{ // Unity chooses the subshader that fits the GPU best
	   Pass { // some shaders require multiple passes
		  GLSLPROGRAM // here begins the part in Unity's GLSL

		  #ifdef VERTEX // here begins the vertex shader

		  void main() // all vertex shaders define a main() function
		  {
			 gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			 // this line transforms the predefined attribute 
			 // gl_Vertex of type vec4 with the predefined
			 // uniform gl_ModelViewProjectionMatrix of type mat4
			 // and stores the result in the predefined output 
			 // variable gl_Position of type vec4.
		  }

	      #endif // here ends the definition of the vertex shader


	   #ifdef FRAGMENT // here begins the fragment shader

	   void main() // all fragment shaders define a main() function
	   {
		  // gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
		  // this fragment shader just sets the output color 
		  // to opaque red (red = 1.0, green = 0.0, blue = 0.0, 
		  // alpha = 1.0)
			 vec2 uv = (fragCoord - 0.5 * iResolution.xy) / iResolution.y;

			uv *= 3.0;
			vec2 gv = fract(uv);
			vec3 col = vec3(gv.xy, 1.);

			col.x *= .2;
			col.y *= .1;
			col.z *= 1.2;

			float m = 0.;
			float t = iTime / 2.;


			float dist = length(uv * 4.);
			for (int x = -2; x <= 2; x++) {
				for (int y = -2; y <= 2; y++) {
					vec2 offset = vec2(x, y);
					float d = length(gv - offset * 0.7)*1.75;
					float tNorm = sin(dist - t) * .5 + 0.5;
					float r = mix(0.35, 1.8, tNorm);

					m += smoothstep(r*1.0001, r, d);
				}
			}

			col.x = mod(m / 1., 1.1);
			col.y = mod(m / 1.1, 1.3);
			col.z = mod(m / 1.3, 1.4);

			fragColor = vec4(col,1.0);
		}

		#endif // here ends the definition of the fragment shader

	ENDGLSL // here ends the part in GLSL 
 }
	}
}