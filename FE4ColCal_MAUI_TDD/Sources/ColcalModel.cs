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

		/// <summary>
		/// 勝率の計算
		/// </summary>
		/// <param name="player">プレイヤーのパラメータ</param>
		/// <param name="enemy">敵のパラメータ</param>
		/// <returns>勝率</returns>
		/// <exception cref="NotImplementedException"></exception>
		public float Calc(Parameter player, Parameter enemy)
		{
			progress = 0;
            //先手後手の決定
            if (player.aspd >= enemy.aspd)
			{
				return OneRound(player, enemy, 0);
			}
			else
			{
                return 1.0f - OneRound(enemy, player, 0);
            }
		}

		//1ラウンドの攻防、勝率を返す
		float OneRound(in Parameter first, in Parameter second, int round)
		{
			if(round >= 8)
			{
				//打ち切り
				CountProgress();
                return 0;
			}

			float ret = 0f;
			//命中する場合
			{
				Parameter secondClone = second.Clone();
				//攻撃でHPを減らす
				if (DealDamage(first, secondClone))
				{
                    CountProgress();
                    ret += ToActualRatio(first.hit);
				}
				else
				{
					//反撃
					{
						Parameter firstClone = first.Clone();
						if (DealDamage(secondClone, firstClone))
						{
                            CountProgress();
                            ret += 0.0f;
						}
						else
						{
							//お互いの攻撃が命中
							//追撃
							ret += Chase(firstClone, secondClone,
								ToActualRatio(firstClone.hit)
								* ToActualRatio(secondClone.hit),
								round);
						}
					}
					{
                        //先攻命中、反撃はずれ
                        ret += Chase(first, secondClone,
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
                    Parameter firstClone = first.Clone();
                    if (DealDamage(second, firstClone))
                    {
                        CountProgress();
                        ret += 0.0f;
                    }
                    else
                    {
						//先攻ハズレ、反撃命中
						ret += Chase(firstClone, second,
							(1.0f - ToActualRatio(firstClone.hit))
							* ToActualRatio(second.hit),
							round);
                    }
                }
				{
                    //お互いにハズレ
                    ret += Chase(first, second,
                            (1.0f - ToActualRatio(first.hit))
							* (1.0f - ToActualRatio(second.hit)),
                            round);
                }
            }
            return ret;
        }

		bool DealDamage(Parameter attack, Parameter defense)
		{
            defense.hp -= Math.Max(attack.atc - defense.def, 1);
			return defense.hp <= 0;
        }

		float Chase(Parameter first, Parameter second, float priorWinrate, int round)
		{
			float ret = 0;
            if (first.chase && first.aspd > second.aspd)
            {
                //追撃命中
                {
                    Parameter secondClone = second.Clone();
                    if (DealDamage(first, secondClone))
                    {
                        CountProgress();
                        ret += priorWinrate
                            * ToActualRatio(first.hit);
                    }
                    else
                    {
                        ret += priorWinrate
                            * ToActualRatio(first.hit)
                            * OneRound(first, secondClone, round + 1);
                    }
                }
                //追撃ハズレ
                {
                    ret += priorWinrate
                        * (1.0f - ToActualRatio(first.hit))
                        * OneRound(first, second, round + 1);
                }
            }
            else
            {
                ret += priorWinrate
                    * OneRound(first, second, round + 1);
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

