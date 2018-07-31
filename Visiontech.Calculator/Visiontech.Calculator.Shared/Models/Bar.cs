using Urho;
using Urho.Actions;
using Urho.Shapes;
using System;

namespace CalcolatoreXamarin.Shared.Models
{
    public class Bar : Component
    {
        static readonly Random random = new Random();

        Node barNode;
        Node dotNode;
        Color color;
        float value;
        readonly float side;

        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                barNode.RunActionsAsync(new EaseBackOut(new ScaleTo(random.Next(2, 5), barNode.Scale.X, value, barNode.Scale.Z)));
            }
        }
        public Bar(float side, Color color)
        {
            this.side = side;
            this.color = color;
            ReceiveSceneUpdates = true;
        }

        public override void OnAttachedToNode(Node node)
        {
            barNode = node.CreateChild();
            barNode.Scale = new Vector3(side, 0, side);

            dotNode = node.CreateChild();
            dotNode.Scale = new Vector3(side, side, side);
            dotNode.Position = new Vector3(0, side / 2f, 0);
            var box = dotNode.CreateComponent<Sphere>();
            box.Color = color;

            base.OnAttachedToNode(node);
        }

        protected override void OnUpdate(float timeStep)
        {
            var pos = barNode.Position;
            var scale = barNode.Scale;
            barNode.Position = new Vector3(pos.X, scale.Y / 2f, pos.Z);
            dotNode.Position = new Vector3(pos.X, (scale.Y + side) / 2f, pos.Z);
        }

    }
}
