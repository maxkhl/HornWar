using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;

namespace Horn_War_II.GameObjects
{
    class Character : BodyObject
    {
        public float SpeedFactor = 8f;

        /// <summary>
        /// Defines a position, the character should try to look at
        /// </summary>
        public Vector2 LookAt { get; set; }


        /// <summary>
        /// Determins the health of this character
        /// </summary>
        public new float Health { get; private set; }

        /// <summary>
        /// Determins if this character is dead
        /// </summary>
        public bool Dead
        {
            get
            {
                return _dead;
            }
            private set
            {
                _dead = value;

                var deathOverlay = TextureOverlays[DeathOverlayIndex];
                deathOverlay.Enabled = value;
                TextureOverlays[DeathOverlayIndex] = deathOverlay;
            }
        }
        private bool _dead = false;
        private int DeathOverlayIndex = -1;

        /// <summary>
        /// String indicating the team of this Character. AI's of teammates with similar strings will not attack each other. Also important for friendly fire
        /// </summary>
        public string Team
        {
            get
            {
                return _Team;
            }
            set
            {
                _Team = value;
            }
        }
        private string _Team = "";

        /// <summary>
        /// Will be filled, if this character is controlled by an AI - otherwise null
        /// </summary>
        public AI.AI AI { get; set; }

        /// <summary>
        /// Activates the characters boost wich will drain stamina until it disables itself or gets disabled (TODO:Stamina)
        /// </summary>
        public bool Boost { get; set; }

        /// <summary>
        /// Distance, this character is able to pickup loose items
        /// </summary>
        public float PickUpDistance { get; set; }

        /// <summary>
        /// Equipted weapon
        /// </summary>
        public virtual Weapon Weapon 
        {
            get
            {
                return _Weapon;
            }
            set
            {
                if (_Weapon != null)
                    _Weapon.onHitCharacter -= OnWeaponHit;

                _Weapon = value;
                
                if(value != null)
                    _Weapon.onHitCharacter += OnWeaponHit;
            }
        }
        private Weapon _Weapon;

        /// <summary>
        /// Fired whenever another character was hit by this characters weapon
        /// </summary>
        public event Weapon.onHitCharacterHandler OnWeaponHit;

        /// <summary>
        /// Gets or sets the speed of this character.
        /// </summary>
        public WalkSpeed Speed { get; set; }

        public enum WalkSpeed
        {
            Full,
            Half,
            Slow,
        }

        /// <summary>
        /// Current skin of this character
        /// </summary>
        public SkinType Skin
        {
            get
            {
                return _Skin;
            }
            set
            {
                _Skin = value;
                switch (_Skin)
                {
                    case SkinType.Cyborg:
                        this.Texture = new hTexture(SceneManager.Game.Content.Load<Texture2D>("Images/Cyborg"), new Vector2(73), 2, 10);
                        //this.Texture.AnimationSequences.Add("Talk", new int[] { 0, 1, 2, 4, 5, 0 });

                        /// Blinking
                        this.TextureOverlays.Add(new TextureOverlay()
                        {
                            Texture = this.Texture,
                            Enabled = false,
                            Offset = Vector2.Zero,
                            AtlasFrame = 1,
                        });
                        BlinkOverlayIndex = this.TextureOverlays.Count - 1;

                        /// Death
                        this.TextureOverlays.Add(new TextureOverlay()
                        {
                            Texture = this.Texture,
                            Enabled = false,
                            Offset = Vector2.Zero,
                            AtlasFrame = 1,
                        });
                        DeathOverlayIndex = this.TextureOverlays.Count - 1;

                        this.Material = BOMaterial.Metal;

                        break;
                    case SkinType.Goblin:
                        this.Texture = new hTexture(SceneManager.Game.Content.Load<Texture2D>("Images/Goblin"),new Vector2(128), 9, 10);
                        this.Texture.AnimationSequences.Add("Talk", new int[] { 0, 1, 2, 4, 5, 0 });

                        /// Blinking
                        this.TextureOverlays.Add(new TextureOverlay()
                        {
                            Texture = this.Texture,
                            Enabled = false,
                            Offset = Vector2.Zero,
                            AtlasFrame = 6,
                        });
                        BlinkOverlayIndex = this.TextureOverlays.Count - 1;

                        /// Death
                        this.TextureOverlays.Add(new TextureOverlay()
                        {
                            Texture = this.Texture,
                            Enabled = false,
                            Offset = Vector2.Zero,
                            AtlasFrame = 7,
                        });
                        DeathOverlayIndex = this.TextureOverlays.Count - 1;

                        this.Material = BOMaterial.Biological;

                        break;
                }
                this.ShapeFromTexture();
            }
        }
        private SkinType _Skin = SkinType.Cyborg;


        public Character(Scenes.GameScene GameScene, PhysicEngine PhysicEngine, SkinType Skin)
            : base(GameScene, PhysicEngine)
        {
            this.Speed = WalkSpeed.Full;
            this.Skin = Skin;

            this.Health = 100;

            //this.Size *= 0.1f;

            this.Damping = 20f;

            this.PickUpDistance = 60;

            this.OnHit += Character_OnHit;
        }

        /// <summary>
        /// Occurs, when character is hit by something
        /// </summary>
        private void Character_OnHit(BodyObject Contact, bool DamagingImpact, float Damage)
        {
            if (DamagingImpact)
                this.Health -= Damage;

            if (this.Health <= 0 && !Dead)
                this.Dead = true;
        }

        /// <summary>
        /// Moves this character into the specified direction.
        /// </summary>
        public void Move(Vector2 Direction)
        {
            if (this.Dead) return; //No movement if character is dead

            if (Direction == Vector2.Zero)
                return;

            Direction.Normalize();

            float WalkStateFactor = 1;
            switch(Speed)
            {
                case WalkSpeed.Full:
                    WalkStateFactor = 1;
                    break;
                case WalkSpeed.Half:
                    WalkStateFactor = 0.5f;
                    break;
                case WalkSpeed.Slow:
                    WalkStateFactor = 0.1f;
                    break;
            }

            this.Push(Direction * (SpeedFactor * WalkStateFactor) * (Boost ? 2 : 1));
        }

        Random random = new Random();
        public override void Update(GameTime gameTime)
        {
            if (this.Dead) return; //No updates if character is dead

            Rotate((float)Math.Atan2(LookAt.Y - this.Position.Y, LookAt.X - this.Position.X), 5f, 5);

            if (random.NextDouble() < 0.005)
                this.Blink();

            base.Update(gameTime);
        }

        /// <summary>
        /// Lets this character say the specified text.
        /// </summary>
        public void Say(string Text)
        {
            if (this.Dead) return; //No talking if character is dead

            // Play talk animation if character has one
            if (this.Texture.AnimationSequences.ContainsKey("Talk"))
                this.Texture.Play(true, "Talk", 5);

            new Label(GameScene, Text, this.Position, 4000, Color.White, Label.Animation.RaiseFade);
        }

        /// <summary>
        /// Lets this character say a random text
        /// </summary>
        public void SayRandom(string[] Text)
        {
            Say(Text[new Random().Next(0, Text.Length)]);
        }

        /// <summary>
        /// Picks up a item near this character
        /// </summary>
        public void PickUp()
        {
            foreach(Weapon weapon in Game.Components.Where(x => x is Weapon && ((Weapon)x).Holder == null))
            {
                var distance = (weapon.Position - this.Position).Length();
                if (distance <= PickUpDistance)
                    weapon.Attach(this);
            }
        }

        private int BlinkOverlayIndex = -1;

        /// <summary>
        /// Lets this character blink
        /// </summary>
        public void Blink()
        {
            if (this.Dead) return; //No blinking if character is dead

            if (TextureOverlays.Count <= BlinkOverlayIndex + 1 && BlinkOverlayIndex >= 0)
            {
                var blinkOverlay = TextureOverlays[BlinkOverlayIndex];
                blinkOverlay.Enabled = true;
                blinkOverlay.Lifetime = 200;
                blinkOverlay.UseLifetime = true;
                TextureOverlays[BlinkOverlayIndex] = blinkOverlay;
            }
        }
		
        /// <summary>
        /// Equipts the given weapon and detaches the currently held one
        /// </summary>
		public void Equipt(Weapon newWeapon)
		{
			if (Weapon != null)
				Weapon.Detach();

			newWeapon.Attach(this);
		}

        /// <summary>
        /// Skin (TODO: Charge at least 5$ per skin)
        /// </summary>
        public enum SkinType
        {
            /// <summary>
            /// Spacey cyborg/knight/something skin
            /// </summary>
            Cyborg,

            /// <summary>
            /// Mean goblin
            /// </summary>
            Goblin,
        }

        public override void Dispose()
        {
            this.OnHit -= Character_OnHit;

            base.Dispose();
        }
    }
}
