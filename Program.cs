using System.Diagnostics;
using System.Linq;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace ScientificGraphicsEngine
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("opening window");
            using var game = new Game(720, 480, "test");
            game.Run();
        }
    }


    public class Game(int width, int height, string title) : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
        private Shader shader;
        private int vbo, vao, ebo;
        private readonly float[] vertices =
        {
            // positions        // colors
            0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
            -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
            0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        private Stopwatch _timer;

        protected override void OnLoad()
        {
            base.OnLoad();
            string vertexPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"assets\shaders\shader.vert");
            string fragmentPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"assets\shaders\shader.frag");
            shader = new Shader(vertexPath, fragmentPath);

            GL.ClearColor(new Color4(0.89f, 0.89f, 1.0f, 1.0f));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer,vertices.Length*sizeof(float),vertices,BufferUsageHint.StaticDraw);
            
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3*sizeof(float));
            GL.EnableVertexAttribArray(1);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length*sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader.Use();

            _timer = new Stopwatch();
            _timer.Start();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if(KeyboardState.IsAnyKeyDown) Close();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();
            float pipa = (float)Math.Sin(_timer.Elapsed.TotalSeconds)/2.0f+0.5f;
            GL.Uniform4(GL.GetUniformLocation(shader.Handle, "inColor"),new Vector4(pipa,pipa,pipa,1.0f) );

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            shader.Dispose();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vbo);
        }
    }
}