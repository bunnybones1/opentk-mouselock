using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ImmediateMode
{
    /// <summary>
    /// Immediate mode is a legacy way of sending commands to the fixed
    /// function pipeline of OpenGL. Since the OpenGL 3.0 specification was
    /// released by Khronos in 2008 this way of working with OpenGL has been
    /// deprecated.
    /// OpenTK does not recomend the use of immediate mode but it continues to
    /// be supported.
    /// </summary>
    sealed class Program : GameWindow
    {
        const float rotation_speed = 0.3f;
        float angleX;
        float angleY;

        bool UseVintageStoryMouseGrabbing = false;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color4.MidnightBlue.R, Color4.MidnightBlue.G, Color4.MidnightBlue.B, Color4.MidnightBlue.A);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            var aspect_ratio = Width / (float)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[OpenTK.Input.Key.Escape])
            {
                CursorGrabbed = false;
                CursorVisible = true;
                return;
            }

            if (UseVintageStoryMouseGrabbing)
            {
                UpdateMousePosition();
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (UseVintageStoryMouseGrabbing)
            {
                if (ignoreMouseMoveEvent)
                {
                    ignoreMouseMoveEvent = false;
                    return;
                }

                // Only way to get actual mouse coordinates?!
                mouseX = e.X;
                mouseY = e.Y;

            }
            else
            {
                angleX += rotation_speed * (float)e.YDelta;
                angleY += rotation_speed * (float)e.XDelta;
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if(e.X > 0.0f && e.X < Width && e.Y > 0.0f && e.Y < Height)
            {
                CursorGrabbed = !CursorGrabbed;
                CursorVisible = !CursorVisible;
            }
        }




        OpenTK.Input.MouseState currentMouseState, previousMouseState;
        int mouseX, mouseY;
        bool ignoreMouseMoveEvent = false;

        void UpdateMousePosition()
        {
            currentMouseState = Mouse.GetState();

            if (!Focused)
            {
                return;
            }

            if (currentMouseState != previousMouseState)
            {
                int xdelta = currentMouseState.X - previousMouseState.X;
                int ydelta = currentMouseState.Y - previousMouseState.Y;
                previousMouseState = currentMouseState;

                angleX += rotation_speed * (float)ydelta;
                angleY += rotation_speed * (float)xdelta;


                if (CursorGrabbed)
                {
                    int centerx = this.Bounds.Left + (Bounds.Width / 2);
                    int centery = Bounds.Top + (Bounds.Height / 2);
                    ignoreMouseMoveEvent = true;
                    Mouse.SetPosition(centerx, centery);
                }
            }
        }






        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Rotate(angleX, 1.0f, 0.0f, 0.0f);
            GL.Rotate(-angleY, 0.0f, 1.0f, 0.0f);
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color4.Silver.R, Color4.Silver.G, Color4.Silver.B);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color4.Honeydew.R, Color4.Honeydew.G, Color4.Honeydew.B);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color4.Moccasin.R, Color4.Moccasin.G, Color4.Moccasin.B);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color4.IndianRed.R, Color4.IndianRed.G, Color4.IndianRed.B);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color4.PaleVioletRed.R, Color4.PaleVioletRed.G, Color4.PaleVioletRed.B);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color4.ForestGreen.R, Color4.ForestGreen.G, Color4.ForestGreen.B);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();

            SwapBuffers();
        }

        [STAThread]
        static void Main()
        {
            var program = new Program();
            program.Run();
        }
    }
}
