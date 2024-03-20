#version 330 core
layout (location = 0) in vec3 FragCoord;
layout (location = 1) in vec3 VertColor;
out vec3 inColor;

void main()
{
    gl_Position = vec4(FragCoord, 1.0);
    inColor = VertColor;
}