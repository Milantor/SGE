using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace ScientificGraphicsEngine
{
    internal class Shader
    {
        public int Handle;
        private bool disposedValue = false;
        public Shader(string vertexPath, string fragmentPath)
        {
            
            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(VertexShader, File.ReadAllText(vertexPath));
            GL.ShaderSource(FragmentShader, File.ReadAllText(fragmentPath));
            
            GL.CompileShader(VertexShader);
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out var success);
            if (success == 0)
            {
                var infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }
           
            GL.CompileShader(FragmentShader);
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                var infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            GL.LinkProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                var infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
        }
        public void Use()
        {
            GL.UseProgram(Handle);
        }
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }
    }
}
