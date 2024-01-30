layout(location=20) uniform int nb_emissives;
layout(location=21) uniform int NB_BOUNCES;

vec3 sample_hemisphere(in float roughness)
{
	// Algo echantillonnage uniforme hemisphere:
	// Z <- rand entre ? et ?
	float Z = random_float();
	// beta <- Z : angle/plan_xy
	float beta = acos(pow(Z, 1.0-roughness));
	// alpha <- rand entre ? et ?
	float alpha = 2.0 * PI * random_float();
	// x,y,z <- alpha,beta : coord polaire -> cartesienne
	float x = sin(beta) * cos(alpha);
    float y = sin(beta) * sin(alpha);
    float z = cos(beta);

	return normalize(vec3(x, y, z));

	// fake !!!!!!
	// return normalize(random_vec3());
}

vec3 random_ray(in vec3 D, in float roughness)
{
	vec3 W;
	// Algo orientation échantillon
	// choisir un W normalisé non colineaire à D
	if(abs(D.x) >= 0.5)
		W = normalize(cross(D, vec3(0.0, 1.0, 0.0)));
    else
        W = normalize(cross(D, vec3(1.0, 0.0, 0.0)));
	// U orthogonal à D et W
	vec3 U = normalize(cross(W, D));
	// V tq U,V,D repère ortho-normé direct
	vec3 V = normalize(cross(D, U));
	// mettre  U,V,D dans une  matrice 3x3 de changement de repère M
	mat3 M = mat3(U, V, D);
	// multiplier votre echantillon par M pour bien l'orienter
	vec3 hemisphereSample = sample_hemisphere(roughness);
	// ici par de matrice 4x4 car pas de translation
	return M * hemisphereSample;
}

vec3 random_path(in vec3 D, in vec3 O)
{
	// Origine et Direction vont être les sources et les directions a chaque rebond
	vec3 Direction = D;
	vec3 Origine = O;
	// final_color va etre le couleur final: cumul des luminances
	vec3 final_color = vec3(0.0);
	// cumul d'attenuation
	float attenuation = 1.0;

	// on souhaite NB_BOUNCES rebonds
	for(int i = 0; i<=NB_BOUNCES; ++i)
	{
		// on lance le rayon depuis Origine dans la nouvelle direction
		traverse_all_bvh(Origine, Direction);
		// si le rayon ne touche rien
		if(!hit())
			// generation d'un ciel procedural
			return final_color + attenuation * mix(vec3(0.1,0.1,0.5),vec3(0.4,0.4,0.9),max(0.0,(0.5+Direction.z)/1.5));
		// sinon, on recupere les informations d'intersections
		vec3 N, P;
		intersection_info(N, P); // normal et point d'intersection
		vec4 color = intersection_color_info(); // couleur
		vec4 mat = intersection_mat_info(); // materiaux
		vec3 local = color.rgb; // couleur local en rgb transfome

		// si l'objet est transparent
		if(color.a < random_float())
		{
			// on rentre dans l'objet
			Direction = refract(Direction, N, 1/1.01);
			N = normalize(N);
			Origine = P-N*BIAS;
			// on recupere la couleur en surface
			final_color = mix(final_color, local*attenuation, color.a);
			// final_color +=  local * attenuation;
			attenuation *= mat.r;

			// on traverse l'objet
			traverse_all_bvh(Origine, Direction);
			intersection_info(N, P);
			color = intersection_color_info();
			mat = intersection_mat_info();
			local = color.rgb;

			// on sort de l'objet
			Direction = random_ray(refract(Direction, -N, 1.01), mat.g);
			// Direction = refract(Direction, -N, 1.01);
			N = normalize(N);
			Origine = P+N*BIAS;
			// on recupere la couleur une fois traverse
			final_color = mix(final_color, local*attenuation, color.a);
			// final_color += local * attenuation;
			attenuation *= mat.r;
			// on incremente le nombre de rebonds
			i++;
			
		} else 
		{
			// on rebondit
			Origine = P+BIAS*N;
			// si on est sur un materiaux rugueux
			if(mat.g > random_float())
				Direction = random_ray(reflect(Direction, N), mat.g);
			else
				Direction = random_ray(N, 0.0);

			// on cumul la lumiere
			final_color += local * attenuation;
			attenuation *= mat.r;
		}	

		// si on touche une lumiere, on renvoi la couleur
		if(mat.b > 0.0) return final_color*mat.b;

	}

	// return final_color;
	return vec3(0.0);
}

vec3 raytrace(in vec3 Dir, in vec3 Orig)   
{
	// init de la graine du random
	srand();
	// calcul de la lumière captée par un chemin aléatoire
	return random_path(normalize(Dir),Orig);
}
