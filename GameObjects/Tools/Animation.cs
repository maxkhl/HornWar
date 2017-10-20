using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.Tools
{
    /// <summary>
    /// Takes care of animating a given value
    /// </summary>
    class Animation
    {        
        /// <summary>
        /// Field, this animation is accessing
        /// </summary>
        public PropertyInfo Field { private set; get; }

        /// <summary>
        /// Instance, this animation is accessing
        /// </summary>
        public object Instance { private set; get; }
        
        /// <summary>
        /// Target Value the animation is moving towards
        /// </summary>
        public object Value { private set; get; }

        /// <summary>
        /// Duration of the animation in MS
        /// </summary>
        public int Duration { private set; get; }
        
        /// <summary>
        /// Easing function the animation is using
        /// </summary>
        public Easing.EaseFunction Function { private set; get; }

        /// <summary>
        /// Time the animation started (first update run after activation)
        /// </summary>
        public TimeSpan AnimationStart { private set; get; }

        /// <summary>
        /// Time the animation will end or ended already
        /// </summary>
        public TimeSpan AnimationEnd { private set; get; }

        /// <summary>
        /// Target Value at the start of the animation
        /// </summary>
        public object ValueStart { private set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Animation"/> is active.
        /// </summary>
        public bool Active { get; set; }

        public Animation(PropertyInfo Field, object Instance)
        {
            
            this.Instance = Instance;
            this.Field = Field;
            this.Active = false;
        }

        public void Animate(object Target, int Duration, Easing.EaseFunction Function)
        {
            this.Reset();
            this.Value = Target;
            this.Duration = Duration;
            this.Function = Function;
            this.Active = true;
        }

        private bool WasActive = false;
        public void Update(GameTime gameTime)
        {
            if(!WasActive && Active)
            {
                this.AnimationStart = gameTime.TotalGameTime;
                this.AnimationEnd = gameTime.TotalGameTime + new TimeSpan(0, 0, 0, 0, (int)Duration);
                this.ValueStart = this.Field.GetValue(this.Instance);
            }

            if(this.Active)
            {
                if (gameTime.TotalGameTime > this.AnimationEnd)
                {
                    this.Field.SetValue(this.Instance, Add(this.ValueStart, this.Value));
                    Reset();

                    OnAnimationDone?.Invoke(this);
                }
                else
                {
                    WasActive = true;
                    var newValue = Animate((float)(gameTime.TotalGameTime - AnimationStart).TotalMilliseconds, this.ValueStart, this.Value, (float)Duration);
                    this.Field.SetValue(this.Instance, newValue);
                }
            }
            else
                WasActive = false;
        }

        public void Reset()
        {
            WasActive = false;
            Active = false;
            this.AnimationStart = TimeSpan.Zero;
            this.AnimationEnd = TimeSpan.Zero;
            this.ValueStart = 0;
            this.Duration = 0;
            this.Value = 0;
        }

        public object Add(object o1, object o2)
        {
            if (o1.GetType() != o2.GetType())
                throw new Exception("Types of values in animation do not match");

            if (typeof(float).IsAssignableFrom(o1.GetType()))
                return (float)o1 + (float)o2;

            if (typeof(Vector2).IsAssignableFrom(o1.GetType()))
                return (Vector2)o1 + (Vector2)o2;

            if (typeof(Vector3).IsAssignableFrom(o1.GetType()))
                return (Vector3)o1 + (Vector3)o2;

            throw new Exception(String.Format("The type {0} is not compatible with animations", o1.GetType().ToString()));
        }

        private object Animate(float Time, object ValueStart, object Value, float Duration)
        {
            if (Value.GetType() != ValueStart.GetType())
                throw new Exception("Types of values in animation do not match");

            if (typeof(float).IsAssignableFrom(Value.GetType()))
                return Easing.Ease(this.Function, Time, (float)ValueStart, (float)Value, (float)Duration);

            if (typeof(Vector2).IsAssignableFrom(Value.GetType()))
                return new Vector2(
                        Easing.Ease(this.Function, Time, ((Vector2)ValueStart).X, ((Vector2)Value).X, (float)Duration),
                        Easing.Ease(this.Function, Time, ((Vector2)ValueStart).Y, ((Vector2)Value).Y, (float)Duration)
                    );

            if (typeof(Vector3).IsAssignableFrom(Value.GetType()))
                return new Vector3(
                        Easing.Ease(this.Function, Time, ((Vector3)ValueStart).X, ((Vector3)Value).X, (float)Duration),
                        Easing.Ease(this.Function, Time, ((Vector3)ValueStart).Y, ((Vector3)Value).Y, (float)Duration),
                        Easing.Ease(this.Function, Time, ((Vector3)ValueStart).Z, ((Vector3)Value).Z, (float)Duration)
                    );

            throw new Exception(String.Format("The type {0} is not compatible with animations", Value.GetType().ToString()));
        }

        public delegate void OnAnimationDoneHandler(Animation sender);
        public event OnAnimationDoneHandler OnAnimationDone;
    }
}
