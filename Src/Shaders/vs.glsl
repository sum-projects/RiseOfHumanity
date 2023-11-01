#version 330

layout (location = 0) in vec3 inPosition;
layout (location = 1) in vec3 inColor;

out vec3 fragColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 project;

void main() {
    gl_Position = project * view * model * vec4(inPosition, 1.0);
    fragColor = inColor;
}
