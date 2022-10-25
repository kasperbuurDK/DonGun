using SharedClassLibrary.Exceptions;

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

        private Position _position;

        private Race_abstract _race;


        // Properties 

        public Position Position 
        {
            get => _position; 
            set { _position = value;  }
        }

        public int Strength
        {
            set { _str = value; }
            get { return _str; }
        }
        public int Dexterity
        {
            set { _dex = value; }
            get { return _dex; }
        }
        public int Constitution
        {
            set { _con = value; }
            get { return _con; }
        }
        public int Intelligence
        {
            set { _int = value; }
            get { return _int; }
        }
        public int Wisdome
        {
            set { _wis = value; }
            get { return _wis; }
        }
        public int Charisma
        {
            set { _cha = value; }
            get { return _cha; }
        }
        public int Health
        {
            set
            {
                _hp = value;
                HealthCurrent = _hp;
            }
            get { return _hp; }
        }
        public int HealthCurrent
        {
            set
            {
                if (_hpCur > _hp)
                    _hpCur = _hp;
                else if (_hpCur < 0)
                    _hpCur = 0;
                else
                    _hpCur = value;
            }
            get { return _hpCur; }
        }
        public int ResourceCurrent
        {
            set
            {
                if (_resourceCur > _resource)
                    _resourceCur = _resource;
                else if (_resourceCur < 0)
                    _resourceCur = 0;
                else
                    _resourceCur = value;
            }
            get { return _resourceCur; }
        }

        public int Resource
        {
            set
            {
                _resource = value;
                ResourceCurrent = _resource;
            }
            get { return _resource; }
        }

        public int Mp { get => _mp; set => _mp = value; }
        public int MpCur { get => _mpCur; set => _mpCur = value; }

        public Race_abstract Race
        {
            set { _race = value; }
            get { return _race; }
        }

        // Constructors
        public Character() { _race = new Race_abstract(0); }

        // Methods
        public void Turn()
        {

        }

        public void Move(char direction, int distance) 
        {

            Position newPos = direction switch
            {
                'N' => new Position(_position.X, _position.Y + distance),
                'E' => new Position(_position.X + distance, _position.Y),
                'S' => new Position(_position.X, _position.Y - distance),
                'W' => new Position(_position.X - distance, _position.Y),
                _ => throw new WrongInputToFunction(),
            };

            _position = newPos;
        }

        
    }
}