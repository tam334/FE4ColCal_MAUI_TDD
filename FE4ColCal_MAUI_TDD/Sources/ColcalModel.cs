using System;
namespace FE4ColCal_MAUI_TDD
{
	public class ColCalModel
	{
		public const int ARENA_FIELD_EFFECT = 20;

		public interface ITestOutputHelper
		{
			public void WriteLine(string str);
		}
		public static ITestOutputHelper outputHelper;

		int progress;


        /// <summary>
        /// パラメータ格納クラス
        /// </summary>
        public class Parameter
		{
			public int hp;
			public int hit;
            public int atc;
			public int def;
			public int aspd;
			public bool chase;
			public bool datk;
            public bool braveW;
			public int crit;
			public int shield;

			public Parameter Clone()
			{
				return (Parameter)this.MemberwiseClone();
			}
        }

		//処理速度向上などのために変更の必要のないものは変更しないものに
		class ConstantParameter
		{
			public int hit { get; private set; }
			public int atc { get; private set; }
            public int def { get; private set; }
            public int aspd { get; private set; }
            public bool chase { get; private set; }
            public bool datk { get; private set; }
            public bool braveW { get; private set; }
            public int crit { get; private set; }
            public int shield { get; private set; }

            public ConstantParameter(Parameter org)
			{
				hit = org.hit;
				atc = org.atc;
				def = org.def;
				aspd = org.aspd;
				chase = org.chase;
				datk = org.datk;
				crit = org.crit;
                braveW = org.braveW;
				shield = org.shield;
			}
        }

		/// <summary>
		/// 勝率の計算
		/// </summary>
		/// <param name="player">プレイヤーのパラメータ</param>
		/// <param name="enemy">敵のパラメータ</param>
		/// <returns>勝率</returns>
		public float Calc(Parameter player, Parameter enemy)
		{
			ConstantParameter playerConst = new ConstantParameter(player);
            ConstantParameter enemyConst = new ConstantParameter(enemy);
			int playerHp = player.hp;
			int enemyHp = enemy.hp;
            progress = 0;
            //先手後手の決定
            if (player.aspd >= enemy.aspd)
			{
				return FirstNormalAttack(playerHp, enemyHp, playerConst, enemyConst, 0);
			}
			else
			{
                return 1.0f - FirstNormalAttack(enemyHp, playerHp, enemyConst, playerConst, 0);
            }
		}

		delegate float AttackFunc(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round);

		//先攻の通常攻撃
		float FirstNormalAttack(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round)
		{
			round++;
			if (round >= 11)
			{
				//打ち切り
				CountProgress();
				return 0;
			}

			return NormalAttackFirst(firstHp, secondHp, first, second, round, FirstDoubleAttack);
		}

		//汎用の通常攻撃
        private float NormalAttackFirst(int attackHp, int defenseHp,
            ConstantParameter attack, ConstantParameter defense,
            int round, AttackFunc nextFunc)
        {
            float ret = 0.0f;
            //命中
            if (attack.hit > 0)
            {
                //大盾
                if(defense.shield > 0)
                {
                    ret += ToActualRatio(attack.hit)
                        * ToActualRatio(defense.shield)
                        * nextFunc(attackHp, defenseHp, attack, defense, round);
                }
                //大盾出ず
                {
                    //必殺が出た
                    if (attack.crit > 0)
                    {
                        int defenseHpAfter = defenseHp;
                        if (DealDamage(ref defenseHpAfter, attack, defense, true))
                        {
                            CountProgress();
                            ret += ToActualRatio(attack.hit)
                                * (1.0f - ToActualRatio(defense.shield))
                                * ToActualRatio(attack.crit);
                        }
                        else
                        {
                            ret += ToActualRatio(attack.hit)
                                * (1.0f - ToActualRatio(defense.shield))
                                * ToActualRatio(attack.crit)
                                * nextFunc(attackHp, defenseHpAfter, attack, defense, round);
                        }
                    }
                    //必殺が出ない
                    {
                        int defenseHpAfter = defenseHp;
                        if (DealDamage(ref defenseHpAfter, attack, defense, false))
                        {
                            CountProgress();
                            ret += ToActualRatio(attack.hit)
                                * (1.0f - ToActualRatio(defense.shield))
                                * (1.0f - ToActualRatio(attack.crit));
                        }
                        else
                        {
                            ret += ToActualRatio(attack.hit)
                                * (1.0f - ToActualRatio(defense.shield))
                                * (1.0f - ToActualRatio(attack.crit))
                                * nextFunc(attackHp, defenseHpAfter, attack, defense, round);
                        }
                    }
                }
            }
            //ハズレ
            {
                ret += (1.0f - ToActualRatio(attack.hit))
                    * nextFunc(attackHp, defenseHp, attack, defense, round);
            }
            return ret;
        }

        //汎用の通常攻撃、後攻版
        private float NormalAttackSecond(int attackHp, int defenseHp,
            ConstantParameter attack, ConstantParameter defense,
            int round, AttackFunc nextFunc)
        {
            float ret = 0.0f;
            //命中
            if (attack.hit > 0)
            {
                //大盾
                if (defense.shield > 0)
                {
                    ret += ToActualRatio(attack.hit)
                        * ToActualRatio(defense.shield)
                        * nextFunc(defenseHp, attackHp, defense, attack, round);
                }
                {
                    //必殺
                    if (attack.crit > 0)
                    {
                        int defenseHpAfter = defenseHp;
                        if (DealDamage(ref defenseHpAfter, attack, defense, true))
                        {
                            CountProgress();
                            ret += 0.0f;
                        }
                        else
                        {
                            ret += ToActualRatio(attack.hit)
                                * ToActualRatio(attack.crit)
                                * nextFunc(defenseHpAfter, attackHp, defense, attack, round);
                        }
                    }
                    {
                        int defenseHpAfter = defenseHp;
                        if (DealDamage(ref defenseHpAfter, attack, defense, false))
                        {
                            CountProgress();
                            ret += 0.0f;
                        }
                        else
                        {
                            ret += ToActualRatio(attack.hit)
                                * (1.0f - ToActualRatio(attack.crit))
                                * nextFunc(defenseHpAfter, attackHp, defense, attack, round);
                        }
                    }
                }
            }
            //ハズレ
            {
                ret += (1.0f - ToActualRatio(attack.hit))
                    * nextFunc(defenseHp, attackHp, defense, attack, round);
            }
            return ret;
        }

        //先攻の連続
        float FirstDoubleAttack(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round)
		{
            float ret = 0.0f;
			if(first.datk || first.braveW)
			{
				//連続発動
				{
                    ret += DoubleRatio(first.aspd, first.braveW)
                        * NormalAttackFirst(firstHp, secondHp, first, second, round, SecondNormalAttack);
				}
				//連続出ず
				{
                    ret += (1.0f - DoubleRatio(first.aspd, first.braveW))
						* SecondNormalAttack(firstHp, secondHp, first, second, round);
                }
			}
			//連続未所持
			else
			{
				ret += SecondNormalAttack(firstHp, secondHp, first, second, round);
            }
			return ret;
        }

		float DoubleRatio(int aspd, bool braveW)
		{
			return braveW ? 1.0f : Math.Max(0.0f, 0.2f + ToActualRatio(aspd));
		}

        //後攻の通常攻撃
        float SecondNormalAttack(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round)
		{
			return NormalAttackSecond(secondHp, firstHp, second, first, round, SecondDoubleAttack);
        }

		bool DealDamage(ref int defHp, ConstantParameter attack,
            ConstantParameter defense, bool crit)
		{
            defHp -= Math.Max(crit ? attack.atc * 2 - defense.def : attack.atc - defense.def,
                1);
			return defHp <= 0;
        }

        //後攻の連続
        float SecondDoubleAttack(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round)
        {
            float ret = 0.0f;
            if (second.datk || second.braveW)
            {
                //連続発動
                {
                    ret += DoubleRatio(second.aspd, second.braveW)
                        * NormalAttackSecond(secondHp, firstHp, second, first, round, FirstChase);
                }
                //連続出ず
                {
                    ret += (1.0f - DoubleRatio(second.aspd, second.braveW))
                        * FirstChase(firstHp, secondHp, first, second, round);
                }
            }
            //連続未所持
            else
            {
                ret += FirstChase(firstHp, secondHp, first, second, round);
            }
            return ret;
        }

        float FirstChase(int firstHp, int secondHp,
			ConstantParameter first, ConstantParameter second, int round)
		{
			float ret = 0;
            if (first.chase && first.aspd > second.aspd)
            {
                ret += NormalAttackFirst(firstHp, secondHp, first, second, round, FirstChaseDouble);
            }
			//追撃なし
            else
            {
                ret += FirstChaseDouble(firstHp, secondHp, first, second, round);
            }
			return ret;
        }

		//先攻追撃連続
		float FirstChaseDouble(int firstHp, int secondHp,
            ConstantParameter first, ConstantParameter second, int round)
		{
            float ret = 0.0f;
            if (first.datk || first.braveW)
            {
                //連続発動
                {
                    ret += DoubleRatio(first.aspd, first.braveW)
                        * NormalAttackFirst(firstHp, secondHp, first, second, round, FirstNormalAttack);
                }
                //連続出ず
                {
                    ret += (1.0f - DoubleRatio(first.aspd, first.braveW))
                        * FirstNormalAttack(firstHp, secondHp, first, second, round);
                }
            }
            //連続未所持
            else
            {
                ret += FirstNormalAttack(firstHp, secondHp, first, second, round);
            }
            return ret;
        }

		void CountProgress()
		{
			progress++;
		}

		float ToActualRatio(int hit)
		{
			return (float)hit / 100f;
		}
	}
}

