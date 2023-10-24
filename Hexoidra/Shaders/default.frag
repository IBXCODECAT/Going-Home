#version 330 core

in vec2 texCoord;
out vec4 FragColor;

uniform sampler2D texture0;

uniform vec3 lightColor;
uniform float ambientStrength;

void main()
{
    vec4 textureColor = texture(texture0, texCoord);
    vec3 ambient = ambientStrength * lightColor; // Calculate ambient lighting
    FragColor = vec4(ambient * textureColor.rgb, textureColor.a);
}
