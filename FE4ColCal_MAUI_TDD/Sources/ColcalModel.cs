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
			return player.aspd >= enemy.aspd ? 1.0f : 0.0f;
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

