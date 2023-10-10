using System.Collections.Generic;

public sealed class CSVParse
{
    //! csvのファイル内容textを解析
    //! @return 解析結果
    public static List<List<string>> Parse(string text)
    {
        List<List<string>> result = new List<List<string>>();
        int currentIndex = 0;
        ParseCSV(result, text, ref currentIndex);
        return result;
    }

    //! 以下の構文木に基づきパース
    //! Csv = 行 {改行 行}* [改行]
    //! 行 = カラム {, カラム}*
    //! カラム = (トークン) | (" 任意の文字列 ")
    //! トークン = ,"\r\nを除いた文字列

    //TODO 任意の文字列内の\"が対応できてないことの対処
    //TODO というか構文木解析のプログラム的に書き方がよくない気がする(終端を子要素で判定してるのとか)

    static void ParseCSV(List<List<string>> dst, string csv, ref int currentIndex)
    {
        ParseRow(dst, csv, ref currentIndex);
        while (CheckNextChar(csv, currentIndex, '\r'))
        {
            ParseNewLine(csv, ref currentIndex);
            ParseRow(dst, csv, ref currentIndex);
        }
        if (CheckNextChar(csv, currentIndex, '\r'))
        {
            ParseNewLine(csv, ref currentIndex);
        }
    }

    static void ParseRow(List<List<string>> dst, string csv, ref int currentIndex)
    {
        List<string> columns = new List<string>();

        columns.Add(ParseColumn(dst, csv, ref currentIndex));
        while (CheckNextChar(csv, currentIndex, ','))
        {
            currentIndex++;
            columns.Add(ParseColumn(dst, csv, ref currentIndex));
        }

        if(columns.Count > 0)
        {
            dst.Add(columns);
        }
    }

    static string ParseColumn(List<List<string>> dst, string csv, ref int currentIndex)
    {
        if(CheckNextChar(csv, currentIndex, '"'))
        {
            return ParseAnyString(dst, csv, ref currentIndex);
        }
        else
        {
            return ParseToken(dst, csv, ref currentIndex);
        }
    }

    static void ParseNewLine(string csv, ref int currentIndex)
    {
        if (CheckNextChar(csv, currentIndex, '\r'))
        {
            currentIndex++;
        }
        if (CheckNextChar(csv, currentIndex, '\n'))
        {
            currentIndex++;
        }
    }

    static string ParseToken(List<List<string>> dst, string csv, ref int currentIndex)
    {
        int adv = 0;
        for (adv = 0; adv + currentIndex < csv.Length; adv++)
        {
            if (CheckNextChar(csv, currentIndex + adv, ',') ||
                CheckNextChar(csv, currentIndex + adv, '\r'))
            {
                break;
            }
        }
        currentIndex += adv;

        return csv.Substring(currentIndex - adv, adv);
    }

    static string ParseAnyString(List<List<string>> dst, string csv, ref int currentIndex)
    {
        if (CheckNextChar(csv, currentIndex, '"'))
        {
            currentIndex++;
        }
        int adv;
        for(adv = 0; adv + currentIndex < csv.Length; adv++)
        {
            if(CheckNextChar(csv, currentIndex + adv, '"'))
            {
                if(CheckNextChar(csv, currentIndex + adv + 1, '"'))
                {
                    adv += 1;
                }
                else
                {
                    break;
                }
            }
        }
        currentIndex += adv + 1;

        return csv.Substring(currentIndex - adv - 1, adv);
    }

    //次の文字が終端でなく、nextであるか
    static bool CheckNextChar(string csv, int currentIndex, char next)
    {
        return csv.Length > currentIndex && csv[currentIndex] == next;
    }
}
