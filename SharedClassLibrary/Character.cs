using Newtonsoft.Json.Linq;
using System.ComponentModel;

using SharedClassLibrary.Exceptions;
using SharedClassLibrary.AuxUtils;
using SharedClassLibrary.Actions;
using static SharedClassLibrary.AuxUtils.GameMasterHelpers;

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
        private int _str = 10;
        private int _dex = 10;
        private int _con = 10;
        private int _int = 10;
        private int _wis = 10;
        private int _cha = 10;

        // 3 main meters
        private int _hpMax = 100;
        private int _hpCur; // Current Hp
        private int _resourceMax = 10; // magic, fury, energy, etc.
        private int _resourceCur; // Current magic, fury, energy, etc.
        private int _mpMax = 10;      // Move point MAX
        private int _mpCur;   // remaining movepoints

        // Navigation
        private Position _position;
        private MoveDirections _facing;
        private int _sightRange;

        //        private List<Character>? _othersInSight = new() { };
        private List<string>? _othersInSight = new() { };
      //  private List<IAnAction>? _possibleActions = new() { };
        private List<string>? _possibleActionsSignatures = new() { };
      //  private List<HelperAction>? _possibleHelperActions = new() { };
        private List<string>? _possibleHelperActionsSignatures = new() { };
      //  private List<OffensiveAction>? _possibleOffensiveActions = new() { };
        private List<string>? _possibleOffensiveActionsSignatures = new() { };


        private Race_abstract _race;
        private int[] _hitModifierProfile = new int[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0, -10, -20, -30, -40, -50, -60, -70, -80, -90, -100 } ;
        protected virtual void SetPropertyField<T>(string propertyName, ref T field, T newValue) 
        { 
            if (!EqualityComparer<T>.Default.Equals(field, newValue)) 
            { 
                field = newValue; 
            } 
        }

        private string _name = "DJON DOE";

        public string Name
        {
            set
            {
                SetPropertyField(nameof(Name), ref _name, value); ;
            }
            get { return _name; }
        }

        public string Signature { get; init; }

        // Properties 
        public Position Position { get => _position; set { _position = value; } }
        public MoveDirections Facing { get => _facing; set { _facing = value; } }
        public int SightRange { get => _sightRange; set { _sightRange = value; } }
        public List<string> OthersInSight { get => _othersInSight; set { _othersInSight = value; } }
        
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
        
        public int HealthMax
        {
            set
            {
                SetPropertyField(nameof(HealthMax), ref _hpMax, value);
                HealthCurrent = _hpMax;
            }
            get { return _hpMax; }
        }
       
        public int HealthCurrent
        {
            set
            {
                if (_hpCur > _hpMax)
                    SetPropertyField(nameof(HealthCurrent), ref _hpCur, _hpMax);
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
                if (_resourceCur > _resourceMax)
                    SetPropertyField(nameof(ResourceCurrent), ref _resourceCur, _resourceMax);
                else if (_resourceCur < 0)
                    SetPropertyField(nameof(ResourceCurrent), ref _resourceCur, 0);
                else
                    SetPropertyField(nameof(ResourceCurrent), ref _resourceCur, value);
            }
            get { return _resourceCur; }
        }

        public int ResourceMax
        {
            set
            {
                SetPropertyField(nameof(ResourceMax), ref _resourceMax, value);
            }
            get { return _resourceMax; }
        }

        public int MpMax { get => _mpMax; set => _mpMax = value; }
        public int MpCur { get => _mpCur; set => _mpCur = value; }

        public Race_abstract Race
        {
            set { SetPropertyField(nameof(Race), ref _race, value); }
            get { return _race; }
        }

        public int Team { get; set; }
      //  public List<HelperAction>? PossibleHelperActions { get => _possibleHelperActions; set => _possibleHelperActions = value; }
      //  public List<OffensiveAction>? PossibleOffensiveActions { get => _possibleOffensiveActions; set => _possibleOffensiveActions = value; }
        public List<string>? PossibleHelperActionsSignatures { get => _possibleHelperActionsSignatures; set => _possibleHelperActionsSignatures = value; }
        public List<string>? PossibleOffensiveActionsSignatures { get => _possibleOffensiveActionsSignatures; set => _possibleOffensiveActionsSignatures = value; }
        public List<string> PossibleActionsSignatures { get => _possibleActionsSignatures; set { _possibleActionsSignatures = value; } }


        public int[] HitModifierProfile { get => _hitModifierProfile; set => _hitModifierProfile = value; }

        // Constructors
        public Character() 
        {
            
            Signature = Guid.NewGuid().ToString();
            _race = new Race_abstract(0);

        }

        

        

        // Methods
        

        public int CalculateDamageGive(int diceValue)
        {
            return RandomRange(0, _str + 1) + diceValue;
        }

        public void RecieveDamage(int damage)
        {

            _hpCur -= damage;
        }


        internal void RecieveHealing(int healing)
        {
            _hpCur += healing;
            if (_hpCur > _hpMax) _hpCur = _hpMax;
        }

        internal int CalculateHealing()
        {
            return (_cha * RandomRange(1,5))/3; 
        }

        internal void RecieveInspiration(int inspiration)
        {
            throw new NotImplementedException();
        }

        internal int CalculateInspiration()
        {
            return (_cha * RandomRange(1,5))/3;
        }

        public override string ToString()
        {
            return String.Format($"[{Strength}, {Dexterity}, {Constitution}, {Wisdome}, {Intelligence}, {Charisma}, {HealthMax}, {ResourceMax}, {Race}]");
        }

       
    }
}