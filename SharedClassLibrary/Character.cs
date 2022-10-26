using Newtonsoft.Json.Linq;
using System.ComponentModel;

using SharedClassLibrary.Exceptions;
using SharedClassLibrary.AuxUtils;
using SharedClassLibrary;

namespace SharedClassLibrary
{
    public class Character
    {
        /* TODO:
         *      Add Character profile image.
         *      
         * 
         */

        // Data fields
        // 6 main stats
        private int _str;
        private int _dex;
        private int _con;
        private int _int;
        private int _wis;
        private int _cha;

        // 3 main meters
        private int _hp;
        private int _hpCur; // Current Hp
        private int _resource; // magic, fury, energy, etc.
        private int _resourceCur; // Current magic, fury, energy, etc.
        private int _mp;      // Move point MAX
        private int _mpCur;   // remaining movepoints

        // Navigation
        private Position _position;
        private MoveDirections _facing;

        private Race_abstract _race;


        protected virtual void SetPropertyField<T>(string propertyName, ref T field, T newValue) 
        { 
            if (!EqualityComparer<T>.Default.Equals(field, newValue)) 
            { 
                field = newValue; 
            } 
        }

        // Properties 

        public Position Position 
        {
            get => _position; 
            set { _position = value;  }
        }

        public MoveDirections Facing 
        {
            get => _facing;
            set { _facing = value; }
        }

        public int Strength
        {
            set { SetPropertyField(nameof(Strength), ref _str, value); }
            get { return _str; }
        }
        public int Dexterity
        {
            set { SetPropertyField(nameof(Dexterity), ref _dex, value); }
            get { return _dex; }
        }
        public int Constitution
        {
            set { SetPropertyField(nameof(Constitution), ref _con, value); }
            get { return _con; }
        }
        public int Intelligence
        {
            set { SetPropertyField(nameof(Intelligence), ref _int, value); }
            get { return _int; }
        }
        public int Wisdome
        {
            set { SetPropertyField(nameof(Wisdome), ref _wis, value); }
            get { return _wis; }
        }
        public int Charisma
        {
            set { SetPropertyField(nameof(Charisma), ref _cha, value); }
            get { return _cha; }
        }
        public int Health
        {
            set
            {
                SetPropertyField(nameof(Health), ref _hp, value);
                HealthCurrent = _hp;
            }
            get { return _hp; }
        }
        public int HealthCurrent
        {
            set
            {
                if (_hpCur > _hp)
                    SetPropertyField(nameof(HealthCurrent), ref _hpCur, _hp);
                else if (_hpCur < 0)
                    SetPropertyField(nameof(HealthCurrent), ref _hpCur, 0);
                else
                    SetPropertyField(nameof(HealthCurrent), ref _hpCur, value);
            }
            get { return _hpCur; }
        }
        public int ResourceCurrent
        {
            set
            {
                if (_resourceCur > _resource)
                    SetPropertyField(nameof(ResourceCurrent), ref _resourceCur, _resource);
                else if (_resourceCur < 0)
                    SetPropertyField(nameof(ResourceCurrent), ref _resourceCur, 0);
                else
                    SetPropertyField(nameof(ResourceCurrent), ref _resourceCur, value);
            }
            get { return _resourceCur; }
        }

        public int Resource
        {
            set
            {
                SetPropertyField(nameof(Resource), ref _resource, value);
            }
            get { return _resource; }
        }

        public int Mp { get => _mp; set => _mp = value; }
        public int MpCur { get => _mpCur; set => _mpCur = value; }

        public Race_abstract Race
        {
            set { SetPropertyField(nameof(Race), ref _race, value); }
            get { return _race; }
        }

        // Constructors
        public Character() { _race = new Race_abstract(0); }

        // Methods
   
        public string Move(MoveDirections direction, int distance) 
        {
            
            if (_facing != direction)
            {
               int costOfTurn = CostOfTurningCharacter(direction);
                if (costOfTurn > _mpCur)
                {
                    return "Not Enoough MP to turn";
                } else
                {
                    _mpCur -= costOfTurn;
                    _facing = direction;
                }
            }

            if (_mpCur < distance*Constants.COSTOFONEMOVE)
            {
                return "Not Enogh MP left to move";
            } else 
            {
                _mpCur -= distance*Constants.COSTOFONEMOVE;
            }

            Position newPos = direction switch
            {
                MoveDirections.North => new Position(_position.X, _position.Y + distance),
                MoveDirections.East => new Position(_position.X + distance, _position.Y),
                MoveDirections.South => new Position(_position.X, _position.Y - distance),
                MoveDirections.West => new Position(_position.X - distance, _position.Y),
                _ => throw new WrongInputToFunction(),
            };


            _position = newPos;
            return "Ok";
        }

        private int CostOfTurningCharacter(MoveDirections turnTowards)
        {
            int costOfturning = 0;  
        
            switch (turnTowards)
            {
                case MoveDirections.North:
                    costOfturning = _facing == MoveDirections.South ? 2 : 1;
                  break;
                case MoveDirections.East:
                    costOfturning = _facing == MoveDirections.West ? 2 : 1;
                    break;
                case MoveDirections.South:
                    costOfturning = _facing == MoveDirections.North ? 2 : 1;
                    break;
                case MoveDirections.West:
                    costOfturning = _facing == MoveDirections.East ? 2 : 1;
                    break;
                default:
                    break;
            }

            return costOfturning;

        }

        public override string ToString()
        {
            return String.Format($"[{Strength}, {Dexterity}, {Constitution}, {Wisdome}, {Intelligence}, {Charisma}, {Health}, {Resource}, {Race}]");
        }
    }
}