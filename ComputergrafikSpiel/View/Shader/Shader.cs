using System;
using System.IO;
using System.Text;
using ComputergrafikSpiel.View.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Shader
{
    [Obsolete]
    public class Shader : IDisposable
    {
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader"/> class.
        /// Creates a new shader.
        /// </summary>
        /// <param name="vertex">Vertex Shader file relative to ./Content/Shaders .</param>
        /// <param name="fragment">Fragment Shader file relative to ./Content/Shaders .</param>
        public Shader(string vertex, string fragment)
        {
            vertex = PrependShaderFilePath(vertex);
            fragment = PrependShaderFilePath(fragment);
            this.ConstructorInputCheck(vertex, fragment);

            var (fragHandle, vertHandle) = Shader.CreateAndCompileShaders(File.ReadAllText(vertex, Encoding.UTF8), File.ReadAllText(fragment, Encoding.UTF8));

            this.Handle = Shader.CreateProgrammAndLinkShaders(true, fragHandle, vertHandle);
        }

        public int Handle { get; private set; }

        public void Use()
        {
            GL.UseProgram(this.Handle);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                GL.DeleteProgram(this.Handle);
                this.isDisposed = true;
            }
        }

        private static (int fragHandle, int vertHandle) CreateAndCompileShaders(string vertexData, string fragmentData)
        {
            int vertHandle = GL.CreateShader(ShaderType.VertexShader);
            int fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(vertHandle, vertexData);
            GL.ShaderSource(fragHandle, fragmentData);

            Shader.Compile(vertHandle);
            Shader.Compile(fragHandle);

            return (fragHandle, vertHandle);
        }

        private static int CreateProgrammAndLinkShaders(bool deleteShaders, params int[] handles )
        {
            int programHandle = GL.CreateProgram();
            foreach (var handle in handles)
            {
                GL.AttachShader(programHandle, handle);
            }

            GL.LinkProgram(programHandle);
            foreach (var handle in handles)
            {
                GL.DetachShader(programHandle, handle);
            }

            if (deleteShaders)
            {
                foreach (var handle in handles)
                {
                    GL.DeleteShader(handle);
                }
            }

            return programHandle;
        }

        private static void Compile(int handle)
        {
            GL.CompileShader(handle);
            string compileInfo = GL.GetShaderInfoLog(handle);
            if (!string.IsNullOrEmpty(compileInfo))
            {
                throw new ShaderCompileException($"Failed to compile shader: {compileInfo}");
            }
        }

        private static string PrependShaderFilePath(string v)
        {
            return Path.Combine("./Content/Shaders/", v);
        }

        private void ConstructorInputCheck(string vertex, string fragment)
        {
            if (!File.Exists(vertex))
            {
                throw new FileNotFoundException("Could not find the path to the vertex shader", vertex);
            }

            if (!File.Exists(fragment))
            {
                throw new FileNotFoundException("Could not find the path to the fragment shader", fragment);
            }
        }
    }
}
