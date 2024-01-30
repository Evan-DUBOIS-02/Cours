/**
* @param in D direction du rayon
* @param in O origine du rayon
* @return couleur calculée en RGB
*/

// 5)
// layout(location=21) uniform vec3 light_pos;
// vec3 compute_lighting_refelect(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
// {
//     vec3 L = normalize(light_pos-P);

// 	// calculate direction from origin to point P
// 	vec3 dir = normalize(O-P);
// 	// calculate reflect dir around normal of P
// 	vec3 dir_reflect = reflect(-dir, N);
// 	// change P position to avoid "flashes"
// 	vec3 new_P = P + 0.01 * dir_reflect;
// 	// run new raytracing in the new direction from the new point P
// 	traverse_all_bvh(new_P, dir_reflect); // on lance un rayon de notre position en direction de L (reflet)
// 	vec4 color_reflect = vec4(0, 0, 0, 1);
// 	float shininess = 0.5;
// 	if(hit()) // si il y'a une intersection
// 	{
// 		color_reflect = intersection_color_info(); // on recupere la couleur
// 		shininess = intersection_mat_info().x;
// 	}

//     float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
//     vec3 R = reflect(-L,N);
//     vec3 E = normalize(O-P);
//     float se = mix(200,2,rough); // roughness -> specular exposant phong
//     float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough*rough);
//             // * (1.0-rough*rough)
//     return mix(color_reflect.xyz, color, shininess) * lamb + spec * vec3(1);
// }

// 6)
// layout(location=21) uniform vec3 light_pos;
// vec3 compute_lighting_refelect_shadow(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
// {
//     vec3 L = normalize(light_pos-P);
// 	vec3 dir = normalize(O-P);
// 	vec3 dir_reflect = reflect(-dir, N);
// 	vec3 new_P = P + 0.01 * dir_reflect;
// 	traverse_all_bvh(new_P, dir_reflect); // on lance un rayon de notre position en direction de L (reflet)
// 	vec4 color_reflect = vec4(0, 0, 0, 1);
// 	float shininess = 0.5;
// 	if(hit()) // si il y'a une intersection
// 	{
// 		color_reflect = intersection_color_info(); // on recupere la couleur
// 		shininess = intersection_mat_info().x;
// 	}

// 	// ombre
// 	new_P = P + 0.01 * L;
// 	traverse_all_bvh(new_P, L);
// 	float ombre = 1.f;
// 	if(hit())
// 	{
// 		ombre = 0.5;
// 	}

//     float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
//     vec3 R = reflect(-L,N);
//     vec3 E = normalize(O-P);
//     float se = mix(200,2,rough); // roughness -> specular exposant phong
//     float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough*rough);
//             // * (1.0-rough*rough)
//     return mix(color_reflect.xyz, mix(vec3(0, 0, 0), color, ombre), shininess) * lamb + spec * vec3(1);
// }

// 7)
// layout(location=21) uniform vec3 light_pos_1;
// layout(location=22) uniform vec3 light_pos_2;
// layout(location=23) uniform vec3 light_pos_3;
// vec3 compute_lighting_refelect_shadow(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough, vec3 light_pos)
// {
//     vec3 L = normalize(light_pos-P);
// 	vec3 dir = normalize(O-P);
// 	vec3 dir_reflect = reflect(-dir, N);
// 	vec3 new_P = P + 0.01 * dir_reflect;
// 	traverse_all_bvh(new_P, dir_reflect); // on lance un rayon de notre position en direction de L (reflet)
// 	vec4 color_reflect = vec4(0, 0, 0, 1);
// 	float shininess = 0.5;
// 	if(hit()) // si il y'a une intersection
// 	{
// 		color_reflect = intersection_color_info(); // on recupere la couleur
// 		shininess = intersection_mat_info().x;
// 	}

// 	// ombre
// 	new_P = P + 0.01 * L;
// 	traverse_all_bvh(new_P, L);
// 	float ombre = 1.f;
// 	if(hit())
// 	{
// 		ombre = 0.5;
// 	}

//     float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
//     vec3 R = reflect(-L,N);
//     vec3 E = normalize(O-P);
//     float se = mix(200,2,rough); // roughness -> specular exposant phong
//     float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough*rough);
//             // * (1.0-rough*rough)
//     return mix(color_reflect.xyz, mix(vec3(0, 0, 0), color, ombre), shininess) * lamb + spec * vec3(1);
// }

// 8)
// layout(location=21) uniform vec3 light_pos;
// vec3 compute_lighting(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
// {
//     vec3 L = normalize(light_pos-P);
//     float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
//     vec3 R = reflect(-L,N);
//     vec3 E = normalize(O-P);
//     float se = mix(200,2,rough); // roughness -> specular exposant phong
//     float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough*rough);
//             // * (1.0-rough*rough)
//     return color * lamb + spec * vec3(1);
// }

// 9)
layout(location=21) uniform vec3 light_pos;
vec3 compute_lighting(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
{
    vec3 L = normalize(light_pos-P);
    float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
    vec3 R = reflect(-L,N);
    vec3 E = normalize(O-P);
    float se = mix(200,2,rough); // roughness -> specular exposant phong
    float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough*rough);
            // * (1.0-rough*rough)
    return color * lamb + spec * vec3(1);
}

vec3 raytrace(in vec3 Dir, in vec3 Orig)   
{
	// 1)
	// just_hit_bvh(Orig,Dir);
	// return mix(vec3(1,0,0),vec3(0,1,0),float(hit()));

	// 2)
	// traverse_all_bvh(Orig, Dir);
	// vec3 color = vec3(0, 0, 0);
	// if(hit()) color = intersection_color_info().rgb;

	// 3)
	// traverse_all_bvh(Orig, Dir);
	// vec3 color = vec3(0, 0, 0);
	// vec3 N,Pg;
	// intersection_info(N, Pg);
	// if(hit()) color = intersection_color_info().rgb;
	// float NL = max(0, dot(N, light_position));
	// color = color*NL;

	// 4)
	// traverse_all_bvh(Orig, Dir);
	// vec3 diffuseColor = vec3(0, 0, 0);
	// vec3 N,Pg;
	// intersection_info(N, Pg);
	// N = normalize(N);
	// vec3 L = normalize(light_position - Pg);
	// if(hit()) diffuseColor = intersection_color_info().rgb;

	// float lambertian = max(0, dot(N, L));
	// float specular = 0.0;

	// if(lambertian > 0.0)
	// {
	// 	vec3 R = reflect(-L, N);
	// 	vec3 V = normalize(-Pg);

	// 	float specAngle = max(dot(R,V), 0.0);
	// 	specular = pow(specAngle, 5);
	// }

	// vec3 ambientColor = diffuseColor;
	// vec3 specularColor = vec3(1, 1, 1);
	// vec4 color = vec4(Ka * ambientColor +
    //              Kd * lambertian * diffuseColor +
    //              Ks * specular * specularColor, 1.0);
	// return color.rgb;

	// 5)
	// traverse_all_bvh(Orig,Dir);
    // vec3 N;
    // vec3 P;
    // intersection_info(N,P);
    // vec4 color = intersection_color_info();
    // vec4 mat = intersection_mat_info();
    // return compute_lighting_refelect(Orig, P,N,color.rgb,mat.g);

	// 6)
	// traverse_all_bvh(Orig,Dir);
    // vec3 N;
    // vec3 P;
    // intersection_info(N,P);
    // vec4 color = intersection_color_info();
    // vec4 mat = intersection_mat_info();
    // return compute_lighting_refelect_shadow(Orig, P,N,color.rgb,mat.g);

	// 7)
	// traverse_all_bvh(Orig,Dir);
    // vec3 N;
    // vec3 P;
    // intersection_info(N,P);
    // vec4 color = intersection_color_info();
    // vec4 mat = intersection_mat_info();
	// vec3 color_1 = compute_lighting_refelect_shadow(Orig, P,N,color.rgb,mat.g, light_pos_1);
	// vec3 color_2 = compute_lighting_refelect_shadow(Orig, P,N,color.rgb,mat.g, light_pos_2);
	// vec3 color_3 = compute_lighting_refelect_shadow(Orig, P,N,color.rgb,mat.g, light_pos_3);
	// vec3 final_color = color_1+color_2+color_3;
    // return final_color/3;

	// 8)
	// // phong normal
	// traverse_all_bvh(Orig,Dir);
    // vec3 N;
    // vec3 P;
    // intersection_info(N,P);
    // vec4 color = intersection_color_info();
    // vec4 mat = intersection_mat_info();
	// vec3 base_color = compute_lighting(Orig,P,N,color.rgb,mat.g);
	// // ajout de la transparence: face interieur
	// vec3 refract_ray = refract(Dir, N, 1.05);
	// N = normalize(N);
	// vec3 fake_P = P-N*0.1;
	// traverse_all_bvh(fake_P, refract_ray);
	// intersection_info(N,P);
	// color = intersection_color_info();
    // mat = intersection_mat_info();
	// vec3 base_color_other_side = compute_lighting(fake_P,P,N,color.rgb,mat.g);
	// // ajout de la transparence: autres objets
	// refract_ray = refract(refract_ray, -N, 1.05);
	// N = normalize(N);
	// fake_P = P+N*0.1;
	// traverse_all_bvh(fake_P, refract_ray);
	// intersection_info(N,P);
	// color = intersection_color_info();
    // mat = intersection_mat_info();
	// vec3 base_color_other_objects = compute_lighting(fake_P,P,N,color.rgb,mat.g);
	// // final color
	// vec3 final_color = mix(mix(base_color, base_color_other_side, 0.6), base_color_other_objects, 0.3);
    // return final_color;

	// 9)
	// phong normal
	traverse_all_bvh(Orig,Dir);
    vec3 N;
    vec3 P;
    intersection_info(N,P);
    vec4 color = intersection_color_info();
    vec4 mat = intersection_mat_info();
	vec3 final_color = compute_lighting(Orig,P,N,color.rgb,mat.g);
	// ajout de la transparence: alternance entre une face interieur et une face exterieur
	vec3 refract_ray = Dir;
	vec3 fake_P;
	int nb_boucle = 0;
	do{
		if(nb_boucle%2 == 0){
			refract_ray = refract(refract_ray, N, 1/1.01);
			N = normalize(N);
			fake_P = P-N*0.1;
		} else {
			refract_ray = refract(refract_ray, -N, 1.01);
			N = normalize(N);
			fake_P = P+N*0.1;
		}
		traverse_all_bvh(fake_P, refract_ray);
		intersection_info(N,P);
		color = intersection_color_info();
		if(!hit()) return final_color;
    	mat = intersection_mat_info();
		if(color.a == 1)
			return mix(final_color, compute_lighting(fake_P,P,N,color.rgb,mat.g), 0.5);
		else
			final_color = mix(final_color, compute_lighting(fake_P,P,N,color.rgb,mat.g), color.a);
			
		nb_boucle++;
	}while(nb_boucle < 1000);
    return final_color;
}

