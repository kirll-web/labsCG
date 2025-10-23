#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec3 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos;

uniform int torusCount;
uniform vec3 torusPositions[10];
uniform float torRadius[10];
uniform float pipeRadius[10];

bool RayIntersectsTorus(
    vec3 startPoint, 
    vec3 direction,
    vec3 position, 
    float R,
    float r,
    out float t
) {
    vec3 p = startPoint - position;
    float maxT = length(direction);
    direction = normalize(d);
   
    float sumDSq = dot(direction, direction);
    float sumPSq = dot(p, p);
    float sumPD = dot(p, direction);
    float R2 = R * R;
    float r2 = r * r;

    float a = sumDSq * sumDSq;
    float b = 4.0 * sumDSq * sumPD;
    float c = 2.0 * sumDSq * (sumPSq - R2 - r2) + 4.0 * sumPD * sumPD + 4.0 * R2 * direction.y * direction.y;
    float dd = 4.0 * sumPD * (sumPSq - R2 - r2) + 8.0 * R2 * p.y * direction.y;
    float e = (sumPSq - R2 - r2) * (sumPSq - R2 - r2) - 4.0 * R2 * (r2 - p.y * p.y);
   
    t = 0.0; 
    const int maxIterations = 10;
    for (int i = 0; i < maxIterations; i++) {
        float f = a * t*t*t*t + b * t*t*t + c * t*t + dd * t + e;
        float df = 4.0 * a * t*t*t + 3.0 * b * t*t + 2.0 * c * t + dd;
        if (abs(f) < 0.001) break;
        t -= f / df;
        t = clamp(t, 0.0, maxT); 
    }

    return t > maxT;
}

bool hasShadow(vec3 startPoint, vec3 direction) {
    for (int i = 0; i < torusCount; i++) {
        float t;
        if (RayIntersectsTorus(
        startPoint, direction,
        torusPositions[i], torRadius[i], pipeRadius[i],
        t
        )) {
            return true;
        }
    }
    return false;
}

vec3 CalculateDiffuse(vec3 norm, vec3 lightDir)
{
    float diff = max(dot(norm, lightDir), 0.0);
    return diff * lightColor;
}

void main()
{
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    vec3 result = lightColor * 0.3;

    if(!hasShadow(FragPos, lightDir))
    {
        result += CalculateDiffuse(norm, lightDir);
    }

    result *= Color;
    FragColor = vec4(result, 1.0);
}
