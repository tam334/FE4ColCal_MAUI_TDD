using System;
namespace FE4ColCal_MAUI_TDD
{
	public class LoadEnemy
	{
        public class Parameter
        {
            public int hp;
            public int atc;
            public int hit;
            public int flee;
            public int def;
            public int mdef;
            public int aspd;
            public bool chase;
            public bool datk;
            public int shield;
            public int crit;
            public bool matk;
        }

        /// <summary>
        /// 敵のデータのロード
        /// </summary>
        /// <param name="chapter">章</param>
        /// <param name="level">闘技レベル</param>
        /// <returns>パラメータ</returns>
        public static LoadEnemy.Parameter LoadEnemyParam(int chapter, int level)
        {
            Parameter param = new Parameter();
            return param;
        }
    }
}

