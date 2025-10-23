


#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec3 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos;

uniform int cubeCount;
uniform vec3 cubeCentres[20];
uniform float cubeSize;

bool IsPointInSquare(vec3 point, vec3 cubeCentre, float size)
{
    float halfSize = size / 2.0;

    vec3 p = point - cubeCentre;

    return (abs(p.x) <= halfSize &&
    abs(p.y) <= halfSize &&
    abs(p.z) <= halfSize);
}


bool IsPointInAnotherObject(vec3 point)
{
    for (int i = 0; i < cubeCount; i++)
    {
        if (IsPointInSquare(point, cubeCentres[i], cubeSize))
        {
            return true;
        }
    }

    return false;
}

bool hasShadow(vec3 startPoint, vec3 direction)
{
    float maxDistance = length(lightPos - startPoint);

    float minDistance = 0.0001;

    int numOfIterations = 500;
    float progress = 0.1;
    for(int i = 0; i < numOfIterations; i++)
    {
        vec3 p = startPoint + direction * progress;
        if (IsPointInAnotherObject(p))
        {
            return true;
        }
        progress += 0.1;
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
