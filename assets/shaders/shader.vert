#version 330 core
layout (location = 0) in vec3 FragCoord;
//layout (location = 1) in vec3 VertColor;
layout (location = 1) in vec2 TexCoord;

uniform mat4 transform;

//out vec3 inColor;
out vec2 inTexCoord;

void main()
{
    gl_Position = vec4(FragCoord, 1.0) * transform;
    inTexCoord = TexCoord;
    //inColor = VertColor;
}