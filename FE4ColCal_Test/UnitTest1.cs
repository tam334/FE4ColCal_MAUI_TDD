namespace FE4ColCal_Test;

using System.Reflection;
using FE4ColCal_MAUI_TDD;

public class UnitTest1
{
    [Fact]
    public void TestPlayerFirstOneKill()
    {
        ColCalModel model = new ColCalModel();

        //プレイヤーが先手を取り、一撃で勝利する
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.99999f,
            1.0f);
    }

    [Fact]
    public void TestEnemyFirstOneKill()
    {
        ColCalModel model = new ColCalModel();
        //相手が先手を取り、一撃で勝利する
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.0f,
            0.0000001f);
    }

    [Fact]
    public void TestPlayerFirstEndurance()
    {
        ColCalModel model = new ColCalModel();

        //プレイヤーが先手を取るが、一撃で倒せず、相手が勝利する
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.0f,
            0.000001f);

        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 3,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.0f,
            0.000001f);
    }

    [Fact]
    public void TestEnemyFirstEndurance()
    {
        ColCalModel model = new ColCalModel();

        //相手が先手を取るが、一撃で倒せず、プレイヤーが勝利する
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.99999f,
            1.0f);

        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 3,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.99999f,
            1.0f);
    }

    [Fact]
    public void TestHit50_50()
    {
        ColCalModel model = new ColCalModel();

        //プレイヤーが先手を取り、お互いの命中50
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 50,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 50,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.5f/0.75f - 0.0001f, //初項0.5、公比0.25の等比級数の無限和で勝率が求まる
            0.5f/0.75f + 0.0001f);

        //相手が先手を取り、お互いの命中50
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 50,
                atc = 1,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 50,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            1 - 0.5f / 0.75f - 0.0001f,
            1 - 0.5f / 0.75f + 0.0001f);
    }

    [Fact]
    public void TestHit60_40()
    {
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 60,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 40,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.6f / (1 - 0.4f * 0.6f) - 0.0001f, //初項0.6、公比0.4*0.6の等比級数の無限和で勝率が求まる
            0.6f / (1 - 0.4f * 0.6f) + 0.0001f);
    }
}
