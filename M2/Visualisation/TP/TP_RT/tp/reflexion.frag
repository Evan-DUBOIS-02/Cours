// WARNIING PAY ATTENTION TO CORRESPONDANCE
// LOCATION <-> FIRST PARAM OF set_uniform_value in .cpp
layout(location=21) uniform vec3 light_pos;
layout(location=23) uniform int NB_BOUNCES;

vec3 compute_shadow(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
{
    // ambiant
    vec3 final = 0.1*color;
    // dir of light
    vec3 L = normalize(light_pos-P);

    // are we in the shadow ?
    just_hit_bvh(P+BIAS*N,L);
    if (hit())
        return final;

    // if not continue normal lighting computation

    float lamb = 0.9*max(0.0,dot(L,N));
    vec3 R = reflect(-L,N);
    vec3 E = normalize(O-P);
    float se = mix(100,2,rough);
    float spec = pow(max(0.0,dot(E,R)),se) * (1.0-0.707*rough*rough);
    final += color * lamb + spec * vec3(1);
    return final;
}


vec3 raytrace(in vec3 Dir, in vec3 Orig)   
{
    vec3 D = Dir;
    vec3 O = Orig;                

    vec3 total = vec3(0);
    float attenu = 1.0;
    for(int i=0; (i<NB_BOUNCES)&&(attenu>0.05); ++i)
    {
        traverse_all_bvh(O,D);
        // si on ne touche rien; calcul d'un faux ciel
        if (!hit())
            return total + attenu * mix (vec3(0.1,0.1,0.5),vec3(0.4,0.4,0.9),max(0.0,(0.5+D.z)/1.5));
        //sinon
        vec3 N;
        vec3 P;
        intersection_info(N,P);
        vec4 color = intersection_color_info();
        vec4 mat = intersection_mat_info();
        vec3 local = compute_shadow(O,P,N,color.rgb,mat.g);

        total += (1.0 - ((NB_BOUNCES>1) ? mat.r : 0.0)) * local * attenu;
        attenu *= mat.r;
        // on rebondit
        O = P+BIAS*N; // il faut se décaler un peu dans la direction de la normale pour eviter les auto-intersections
        D = reflect(D,N);
    }
    return total;

    traverse_all_bvh(Orig,Dir);
    vec3 N;
    vec3 P;
    intersection_info(N,P);
    vec4 color = intersection_color_info();
    vec4 mat = intersection_mat_info();
    return compute_shadow(Orig,P,N,color.rgb,mat.g);
}