using System;
namespace FE4ColCal_MAUI_TDD
{
	public class ColCalModel
	{
		public const int ARENA_FIELD_EFFECT = 20;

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
		float OneRound(Parameter first, Parameter second, int round)
		{
			if(round >= 100)
			{
				//打ち切り
				return 0;
			}

			float ret = 0f;
            //命中する場合
            if (first.hit > 0)
            {
                Parameter secondClone = second.Clone();
                //攻撃でHPを減らす
                DealDamage(first, secondClone);

                if (secondClone.hp <= 0)
                {
                    ret += ToActualRatio(first.hit);
                }
                else
                {
					//反撃
					if (secondClone.hit > 0)
					{
						Parameter firstClone = first.Clone();
                        DealDamage(secondClone, firstClone);
						if (firstClone.hp <= 0)
						{
							ret += 0.0f;
						}
						else
						{
							//お互いの攻撃が命中
							ret += ToActualRatio(firstClone.hit) * ToActualRatio(secondClone.hit) * OneRound(firstClone, secondClone, round + 1);
                        }
					}
					if(secondClone.hit < 100)
					{
						//先攻命中、反撃はずれ
                        ret += ToActualRatio(first.hit) * (1.0f - ToActualRatio(secondClone.hit)) * OneRound(first, secondClone, round + 1);
                    }
                }
            }
            //当たらない場合
            if (first.hit < 100)
            {
                //反撃
                if (second.hit > 0)
                {
                    Parameter firstClone = first.Clone();
                    DealDamage(second, firstClone);
                    if (firstClone.hp <= 0)
                    {
                        ret += 0.0f;
                    }
                    else
                    {
						//先攻ハズレ、反撃命中
                        ret += (1.0f - ToActualRatio(first.hit)) * ToActualRatio(second.hit) * OneRound(firstClone, second, round + 1);
                    }
                }
				if(second.hit < 100)
				{
                    //お互いにハズレ
                    ret += (1.0f - ToActualRatio(first.hit)) * (1.0f - ToActualRatio(second.hit)) * OneRound(first, second, round + 1);
                }
            }
            return ret;
        }

		void DealDamage(Parameter attack, Parameter defense)
		{
            defense.hp -= Math.Max(attack.atc - defense.def, 1);
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

