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
				shield = org.shield;
			}
        }

		/// <summary>
		/// 勝率の計算
		/// </summary>
		/// <param name="player">プレイヤーのパラメータ</param>
		/// <param name="enemy">敵のパラメータ</param>
		/// <returns>勝率</returns>
		/// <exception cref="NotImplementedException"></exception>
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
				return OneRound(playerHp, enemyHp, playerConst, enemyConst, 0);
			}
			else
			{
                return 1.0f - OneRound(enemyHp, playerHp, enemyConst, playerConst, 0);
            }
		}

		//1ラウンドの攻防、勝率を返す
		float OneRound(int firstHp, int secondHp, ConstantParameter first, ConstantParameter second, int round)
		{
			if(round >= 12)
			{
				//打ち切り
				CountProgress();
                return 0;
			}

			float ret = 0f;
			//命中する場合
			{
				//攻撃でHPを減らす
				int secondHpAfter = secondHp;
				if (DealDamage(ref secondHpAfter, first, second))
				{
                    CountProgress();
                    ret += ToActualRatio(first.hit);
				}
				else
				{
					//反撃
					{
						int firstHpAfter = firstHp;
						if (DealDamage(ref firstHpAfter, second, first))
						{
                            CountProgress();
                            ret += 0.0f;
						}
						else
						{
							//お互いの攻撃が命中
							//追撃
							ret += Chase(firstHpAfter, secondHpAfter,
								first, second,
								ToActualRatio(first.hit)
								* ToActualRatio(second.hit),
								round);
						}
					}
					{
                        //先攻命中、反撃はずれ
                        ret += Chase(firstHp, secondHpAfter, first, second,
                            ToActualRatio(first.hit)
                            * (1.0f - ToActualRatio(second.hit)),
                            round);
					}
				}
			}
			//当たらない場合
			{
                //反撃
                {
					int firstHpAfter = firstHp;
                    if (DealDamage(ref firstHpAfter, second, first))
                    {
                        CountProgress();
                        ret += 0.0f;
                    }
                    else
                    {
						//先攻ハズレ、反撃命中
						ret += Chase(firstHpAfter, secondHp, first, second,
							(1.0f - ToActualRatio(first.hit))
							* ToActualRatio(second.hit),
							round);
                    }
                }
				{
                    //お互いにハズレ
                    ret += Chase(firstHp, secondHp, first, second,
                            (1.0f - ToActualRatio(first.hit))
							* (1.0f - ToActualRatio(second.hit)),
                            round);
                }
            }
            return ret;
        }

		bool DealDamage(ref int defHp, ConstantParameter attack, ConstantParameter defense)
		{
            defHp -= Math.Max(attack.atc - defense.def, 1);
			return defHp <= 0;
        }

		float Chase(int firstHp, int secondHp,
			ConstantParameter first, ConstantParameter second,
			float priorWinrate, int round)
		{
			float ret = 0;
            if (first.chase && first.aspd > second.aspd)
            {
                //追撃命中
                {
					int secondHpAfter = secondHp;
                    if (DealDamage(ref secondHpAfter, first, second))
                    {
                        CountProgress();
                        ret += priorWinrate
                            * ToActualRatio(first.hit);
                    }
                    else
                    {
                        ret += priorWinrate
                            * ToActualRatio(first.hit)
                            * OneRound(firstHp, secondHpAfter, first, second, round + 1);
                    }
                }
                //追撃ハズレ
                {
                    ret += priorWinrate
                        * (1.0f - ToActualRatio(first.hit))
                        * OneRound(firstHp, secondHp, first, second, round + 1);
                }
            }
            else
            {
                ret += priorWinrate
                    * OneRound(firstHp, secondHp, first, second, round + 1);
            }
			return ret;
        }

		void CountProgress()
		{
			progress++;
			if(outputHelper != null)
			{
				outputHelper.WriteLine("Progress: " + progress + " / 65535");

            }
		}

		float ToActualRatio(int hit)
		{
			return (float)hit / 100f;
		}

		/// <summary>
		/// 敵のデータのロード
		/// </summary>
		/// <param name="chapter">章</param>
		/// <param name="level">闘技レベル</param>
		/// <returns>パラメータ</returns>
		/// <exception cref="NotImplementedException"></exception>
		public Parameter LoadEnemyParam(int chapter, int level)
		{
			throw new NotImplementedException();
		}
	}
}

