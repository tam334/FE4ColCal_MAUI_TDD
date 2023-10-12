using System;
namespace FE4ColCal_MAUI_TDD
{
	public class ColCalModel
	{
		public const int ARENA_FIELD_EFFECT = 20;

        public ColCalModel()
        {
            onReportProgress = null;
        }

		public interface ITestOutputHelper
		{
			public void WriteLine(string str);
		}
		public static ITestOutputHelper outputHelper;

        public delegate void OnReportProgress(UInt128 current, UInt128 max);

        public OnReportProgress onReportProgress;

        UInt128 progress;
        const int roundMax = 10;
        readonly UInt128 progressMax = (UInt128)Math.Pow(3, roundMax * 6);


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
			public float hit { get; private set; }
			public int atc { get; private set; }
            public int def { get; private set; }
            public int aspd { get; private set; }
            public bool chase { get; private set; }
            public int datk { get; private set; }
            public int crit { get; private set; }

            public ConstantParameter(Parameter org, int opponentShield)
			{
				hit = org.hit * (100 - opponentShield) / 10000f;
				atc = org.atc;
				def = org.def;
				aspd = org.aspd;
				chase = org.chase;
				datk = org.braveW ? 100 : org.datk ? org.aspd + 20 : 0;
				crit = org.crit;
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
			ConstantParameter playerConst = new ConstantParameter(player, enemy.shield);
            ConstantParameter enemyConst = new ConstantParameter(enemy, player.shield);
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
            if (round >= roundMax + 1)
			{
				//打ち切り
				CountProgress(round - 1, 1);
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
            {
                //必殺が出た
                if (attack.crit > 0)
                {
                    int defenseHpAfter = defenseHp;
                    if (DealDamage(ref defenseHpAfter, attack, defense, true))
                    {
                        CountProgress(round, 1);
                        ret += attack.hit
                            * ToActualRatio(attack.crit);
                    }
                    else
                    {
                        ret += attack.hit
                            * ToActualRatio(attack.crit)
                            * nextFunc(attackHp, defenseHpAfter, attack, defense, round);
                    }
                }
                //必殺が出ない
                {
                    int defenseHpAfter = defenseHp;
                    if (DealDamage(ref defenseHpAfter, attack, defense, false))
                    {
                        CountProgress(round, 1);
                        ret += attack.hit
                            * (1.0f - ToActualRatio(attack.crit));
                    }
                    else
                    {
                        ret += attack.hit
                            * (1.0f - ToActualRatio(attack.crit))
                            * nextFunc(attackHp, defenseHpAfter, attack, defense, round);
                    }
                }
            }
            //ハズレ
            {
                ret += (1.0f - attack.hit)
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
            {
                //必殺
                if (attack.crit > 0)
                {
                    int defenseHpAfter = defenseHp;
                    if (DealDamage(ref defenseHpAfter, attack, defense, true))
                    {
                        CountProgress(round, 1);
                        ret += 0.0f;
                    }
                    else
                    {
                        ret += attack.hit
                            * ToActualRatio(attack.crit)
                            * nextFunc(defenseHpAfter, attackHp, defense, attack, round);
                    }
                }
                {
                    int defenseHpAfter = defenseHp;
                    if (DealDamage(ref defenseHpAfter, attack, defense, false))
                    {
                        CountProgress(round, 1);
                        ret += 0.0f;
                    }
                    else
                    {
                        ret += attack.hit
                            * (1.0f - ToActualRatio(attack.crit))
                            * nextFunc(defenseHpAfter, attackHp, defense, attack, round);
                    }
                }
            }
            //ハズレ
            {
                ret += (1.0f - attack.hit)
                    * nextFunc(defenseHp, attackHp, defense, attack, round);
            }
            return ret;
        }

        //先攻の連続
        float FirstDoubleAttack(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round)
		{
            float ret = 0.0f;
			if(first.datk > 0)
			{
				//連続発動
				{
                    ret += ToActualRatio(first.datk)
                        * NormalAttackFirst(firstHp, secondHp, first, second, round, SecondNormalAttack);
				}
				//連続出ず
				{
                    //枝刈りされた子ノードの数だけprogressを追加
                    CountProgress(round, 2);
                    ret += (1.0f - ToActualRatio(first.datk))
						* SecondNormalAttack(firstHp, secondHp, first, second, round);
                }
			}
			//連続未所持
			else
			{
                CountProgress(round, 2);
                ret += SecondNormalAttack(firstHp, secondHp, first, second, round);
            }
			return ret;
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
            if (second.datk > 0)
            {
                //連続発動
                {
                    ret += ToActualRatio(second.datk)
                        * NormalAttackSecond(secondHp, firstHp, second, first, round, FirstChase);
                }
                //連続出ず
                {
                    CountProgress(round, 2);
                    ret += (1.0f - ToActualRatio(second.datk))
                        * FirstChase(firstHp, secondHp, first, second, round);
                }
            }
            //連続未所持
            else
            {
                CountProgress(round, 2);
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
                CountProgress(round, 5);
                ret += FirstNormalAttack(firstHp, secondHp, first, second, round);
            }
			return ret;
        }

		//先攻追撃連続
		float FirstChaseDouble(int firstHp, int secondHp,
            ConstantParameter first, ConstantParameter second, int round)
		{
            float ret = 0.0f;
            if (first.datk > 0)
            {
                //連続発動
                {
                    ret += ToActualRatio(first.datk)
                        * NormalAttackFirst(firstHp, secondHp, first, second, round, FirstNormalAttack);
                }
                //連続出ず
                {
                    CountProgress(round, 2);
                    ret += (1.0f - ToActualRatio(first.datk))
                        * FirstNormalAttack(firstHp, secondHp, first, second, round);
                }
            }
            //連続未所持
            else
            {
                CountProgress(round, 2);
                ret += FirstNormalAttack(firstHp, secondHp, first, second, round);
            }
            return ret;
        }

		void CountProgress(int round, float times)
		{
			progress += (UInt128)(Math.Pow(3, 6 * (roundMax - round)) * times);
            if(onReportProgress != null)
            {
                onReportProgress(progress, progressMax);
            }
		}

		float ToActualRatio(int hit)
		{
			return (float)hit / 100f;
		}
	}
}

