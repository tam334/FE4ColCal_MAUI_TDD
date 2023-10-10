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
            Task<Stream> task = FileSystem.Current.OpenAppPackageFileAsync("闘技場データ.csv");
            task.Wait();
            StreamReader reader = new StreamReader(task.Result);
            string text = reader.ReadToEnd();
            List<List<string>> csv = CSVParse.Parse(text);

            int row = -1;
            for (int i = 0; i < csv.Count; i++)
            {
                if (csv[i].Count > 0 && csv[i][0] == string.Format("{0}-{1}", chapter, level))
                {
                    row = i;
                    break;
                }
            }
            List<string> targetRow = csv[row];
            List<string> topRow = csv[0];
            Parameter param = new Parameter()
            {
                hp = int.Parse(targetRow[topRow.IndexOf("HP")]),
                atc = int.Parse(targetRow[topRow.IndexOf("攻撃")]),
                hit = int.Parse(targetRow[topRow.IndexOf("命中")]),
                flee = int.Parse(targetRow[topRow.IndexOf("回避")]),
                def = int.Parse(targetRow[topRow.IndexOf("守備")]),
                mdef = int.Parse(targetRow[topRow.IndexOf("魔防")]),
                aspd = int.Parse(targetRow[topRow.IndexOf("攻撃速度")]),
                chase = targetRow[topRow.IndexOf("追撃")] == "o",
                datk = targetRow[topRow.IndexOf("連続")] == "o",
                shield = int.Parse(targetRow[topRow.IndexOf("大盾発動率")]),
                crit = int.Parse(targetRow[topRow.IndexOf("必殺率")]),
                matk = targetRow[topRow.IndexOf("魔法攻撃")] == "o",
            };
            return param;
        }
    }
}

