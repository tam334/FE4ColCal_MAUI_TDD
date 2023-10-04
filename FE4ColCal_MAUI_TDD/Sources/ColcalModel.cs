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
			for (int round = 0; round < 100; round++)
			{
				if (player.aspd >= enemy.aspd)
				{

					//攻撃でHPを減らす
					DealDamage(player, enemy);

					if (enemy.hp <= 0)
					{
						return 1.0f;
					}
					else
					{
						//反撃
						DealDamage(enemy, player);
						if (player.hp <= 0)
						{
							return 0.0f;
						}
					}
				}
				else
				{
                    DealDamage(enemy, player);
                    if (player.hp <= 0)
                    {
						return 0.0f;
                    }
                    else
                    {
                        //反撃
                        DealDamage(player, enemy);
                        if (enemy.hp <= 0)
                        {
							return 1.0f;
                        }
                    }
                }
			}
			return 0.0f;
		}

		//ダメージ処理
		void DealDamage(Parameter attack, Parameter defense)
		{
            defense.hp -= Math.Max(attack.atc - defense.def, 1);
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

