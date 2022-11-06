using System.ComponentModel;
using SharedClassLibrary.Exceptions;
using SharedClassLibrary.AuxUtils;
using SharedClassLibrary.Actions;
using System.Text.Json.Serialization;

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
        private int _sightRange;

        private int _team;

        private List<Character>? _othersInSight;
        private List<IAnAction>? _possibleActions;
        private List<HelperAction>? _possibleHelperActions;
        private List<OffensiveAction>? _possibleOffensiveActions;


        private Race_abstract _race;

        protected virtual void SetPropertyField<T>(string propertyName, ref T field, T newValue) 
        { 
            if (!EqualityComparer<T>.Default.Equals(field, newValue)) 
            { 
                field = newValue; 
            } 
        }

        // Properties 
        public Position Position { get => _position; set { _position = value; } }

        public MoveDirections Facing { get => _facing; set { _facing = value; } }

        public int SightRange { get => _sightRange; set { _sightRange = value; } }

        [JsonIgnore] // DOES NOT WORK, NEEDS TO BE REMOVED!
        public List<Character> OthersInSight { get => _othersInSight; set { _othersInSight = value; } }

        [JsonIgnore] // DOES NOT WORK, NEEDS TO BE REMOVED!
        public List<IAnAction> PossibleActions { get => _possibleActions; set { _possibleActions = value; } }

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

        public int Team { get => _team; set => _team = value; }

        public List<HelperAction>? PossibleHelperActions { get => _possibleHelperActions; set => _possibleHelperActions = value; }
        public List<OffensiveAction>? PossibleOffensiveActions { get => _possibleOffensiveActions; set => _possibleOffensiveActions = value; }

        // Constructors
        public Character() { _race = new Race_abstract(0); }

        // Methods
        public void UpdatePossibleActions()
        {
            if (_othersInSight != null)
            {
                foreach (Character otherCharacter in _othersInSight)
                {
                    _possibleHelperActions = new List<HelperAction>();
                    _possibleOffensiveActions = new List<OffensiveAction>();
                    if (otherCharacter.Team == _team)
                    {
                        HelperAction aHelperAction = new();
                        _possibleHelperActions.Add(aHelperAction);
                    }
                    else if (otherCharacter.Team != _team)
                    {
                        OffensiveAction anOffensiveAction = new();
                        _possibleOffensiveActions.Add(anOffensiveAction);
                    }

                }
            }
            
        }


        public override string ToString()
        {
            return String.Format($"[{Strength}, {Dexterity}, {Constitution}, {Wisdome}, {Intelligence}, {Charisma}, {Health}, {Resource}, {Race}]");
        }

        
    }
}