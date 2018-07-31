using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using Urho;
using Urho.Actions;

namespace CalcolatoreXamarin.Shared.Models
{
    public class Dots : Application
    {

        static readonly Random random = new Random();

        Node plotNode;
        Camera camera;
        Octree octree;
        private readonly float size = 10;

        public ICollection<threeDimensionalPointDTO> Points { get; set; }

        public Func<threeDimensionalPointDTO, double> Mapping { get; set; }

        public Dots(ApplicationOptions options = null) : base(SetOptions(options)) {

            if (!(options is null)) {
                if (options is DotsOptions)
                {
                    Points = (options as DotsOptions).Points;
                    Mapping = (options as DotsOptions).Mapping;
                }
            }

        }

        private static ApplicationOptions SetOptions(ApplicationOptions options)
        {
            options.TouchEmulation = true;
            return options;
        }

        protected override async void Start()
        {
            base.Start();

            // 3D scene with Octree
            var scene = new Scene(Context);
            octree = scene.CreateComponent<Octree>();

            // Camera
            var cameraNode = scene.CreateChild();
            cameraNode.Position = new Vector3(0, size * 2, size);
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.LookAt(Vector3.Zero, Vector3.Up);

            // Light
            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = size * size;
            light.Brightness = 1.3f;

            // Viewport
            var viewport = new Viewport(Context, scene, camera);
            Renderer.SetViewport(0, viewport);

            plotNode = scene.CreateChild();

            if (!(Points is null))
            {

                int quantity = Convert.ToInt32(Math.Sqrt(Points.Count));
                float step = size / Convert.ToSingle(quantity);

                double max = Points.Select(Mapping).Max();
                double min = Points.Select(Mapping).Min();
                double range = (max - min) / 6;

                if (range > 0)
                {
                    double doubleRange = range * 2;
                    double tripleRange = range * 3;
                    double fiveRange = range * 5;

                    foreach (threeDimensionalPointDTO point in Points)
                    {

                        Color color = Color.Black;

                        double value = Mapping.Invoke(point) - min;

                        switch (Math.Floor(value / range))
                        {

                            case -1:
                            case 0:
                                color = new Color(Convert.ToSingle(value / range), Convert.ToSingle(value / range), 1);
                                break;
                            case 1:
                                color = new Color(Convert.ToSingle((doubleRange - value) / range), 1, 1);
                                break;
                            case 2:
                                color = new Color(0, 1, Convert.ToSingle((tripleRange - value) / range));
                                break;
                            case 3:
                                color = new Color(Convert.ToSingle((value - tripleRange) / range), 1, 0);
                                break;
                            case 4:
                                color = new Color(1, Convert.ToSingle((fiveRange - value) / range), 0);
                                break;
                            default:
                                color = new Color(1, 0, Convert.ToSingle((value - fiveRange) / range));
                                break;

                        }
                        var boxNode = plotNode.CreateChild();
                        boxNode.Position = new Vector3(Convert.ToSingle(point.x * step), 0, Convert.ToSingle(-point.y * step));
                        var box = new Bar(step, color);
                        boxNode.AddComponent(box);
                        box.Value = Convert.ToSingle(value);

                    }
                }

            }

            await plotNode.RunActionsAsync(new EaseBackOut(new RotateBy(2f, 0, 360, 0)));
        }

        protected override void OnUpdate(float timeStep)
        {
            
            if (Input.NumTouches == 1)
            {
                var touch = Input.GetTouch(0);

                camera.Node.RotateAround(plotNode.Position, new Quaternion(camera.Node.Up * touch.Delta.X, 360) * new Quaternion(camera.Node.Right * touch.Delta.Y, 360), TransformSpace.Parent);

            } else if (Input.NumTouches == 2) {

                TouchState touch1 = Input.GetTouch(0);
                TouchState touch2 = Input.GetTouch(1);

                double lastDistance = Math.Sqrt(Math.Pow(touch1.LastPosition.X - touch2.LastPosition.X, 2) + Math.Pow(touch1.LastPosition.Y - touch2.LastPosition.Y, 2));
                double distance = Math.Sqrt(Math.Pow(touch1.Position.X - touch2.Position.X, 2) + Math.Pow(touch1.Position.Y - touch2.Position.Y, 2));
                double variation = Math.Sqrt(Math.Pow(touch1.Delta.X + touch2.Delta.X, 2) + Math.Pow(touch1.Delta.Y + touch2.Delta.Y, 2));

                camera.Zoom += Convert.ToSingle(camera.Zoom * Math.Sign(distance - lastDistance) * variation / distance);

            }
            
            base.OnUpdate(timeStep);
        }
    }

}
