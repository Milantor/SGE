#version 330 core
layout (location = 0) in vec3 FragCoord;
//layout (location = 1) in vec3 VertColor;
layout (location = 1) in vec2 TexCoord;

//out vec3 inColor;
out vec2 inTexCoord;

void main()
{
    inTexCoord = TexCoord;
    gl_Position = vec4(FragCoord, 1.0);
    //inColor = VertColor;
}