using System;
using System.Runtime.InteropServices;

namespace SharedClasses
{
	public class Character_abstract
	{
        // Data fields
        // 6 main stats
        private int _str;
        private int _dex;
        private int _con;
        private int _int;
        private int _wis;
        private int _cha;

        // 2 main meters
        private int _hp;
        private int _resource; // magic, fury, energy, etc.



        // Properties 
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
            set { _hp = value; }
            get { return _hp; }
        }

        public int Resource
        {
            set { _resource = value; }
            get { return _resource; }
        }

        // Constructors
        public Character_abstract() {}

        // Methods
    }
}
