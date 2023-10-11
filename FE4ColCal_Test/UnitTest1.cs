namespace FE4ColCal_Test;
using Xunit;
using Xunit.Abstractions;
using System.Reflection;
using FE4ColCal_MAUI_TDD;

public class UnitTest1
{
    ITestOutputHelper outputHelper;
    public UnitTest1(ITestOutputHelper outputHelper)
    {
        this.outputHelper = outputHelper;
    }

    public class XunitOutputHelper : ColCalModel.ITestOutputHelper
    {
        ITestOutputHelper outputHelper;
        public XunitOutputHelper(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        public void WriteLine(string str)
        {
            outputHelper.WriteLine(str);
        }
    }

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

    [Fact]
    public void TestHit60_40_Endurance()
    {
        //お互いに耐久2発の場合
        //例えば先攻が勝つ場合、
        //先攻-先攻
        //先行-後攻-先行
        //後攻-先行-先行
        //の3パターンの当たり方をするので、それぞれについて確率計算して
        //総和を求めれば良い。
        ColCalModel model = new ColCalModel();
        float playerHit = 0.6f / (1 - 0.4f * 0.6f); //初項0.6、公比0.4*0.6の等比級数の無限和で勝率が求まる
        float enemyHit = 0.4f / (1 - 0.4f * 0.6f);
        float winrate = playerHit * 0.6f * playerHit
            + playerHit * enemyHit * playerHit
            + 0.4f * enemyHit * playerHit * 0.6f * playerHit;
        ColCalModel.outputHelper = new XunitOutputHelper(outputHelper);
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
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
                hp = 2,
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
            winrate - 0.0001f,
            winrate + 0.0001f);
    }

    [Fact]
    public void TestPlayerChase()
    {
        //追撃の際のテスト
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 2,
                chase = true,
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
            1.0f - 0.0001f,
            1.0f);

        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = true,
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
            0,
            0.0001f);
    }

    [Fact]
    public void TestArdanShark()
    {
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 36,
                hit = 60,
                atc = 21,
                def = 13,
                aspd = 3,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 40,
                hit = 77,
                atc = 20,
                def = 8,
                aspd = 10,
                chase = true,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.097f,
            0.098f);
    }

    [Fact]
    public void TestPlayerDouble()
    {
        //連続、プレイヤーが20%で勝利
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 1,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 0,
                chase = false,
                datk = true,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = -1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.199f,
            0.201f);
    }

    [Fact]
    public void TestCuannEmir()
    {
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 48,
                atc = 2,
                def = 1,
                aspd = 0,
                chase = false,
                datk = true,
                crit = 0,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 75,
                atc = 2,
                def = 1,
                aspd = 16,
                chase = false,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.217f,
            0.218f);
    }

    [Fact]
    public void TestCriticalPlayerWin()
    {
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 0,
                chase = false,
                datk = false,
                crit = 10,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 3,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = true,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.1f - 0.001f,
            0.1f + 0.001f);
    }

    [Fact]
    public void TestCriticalEnemyWin()
    {
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 3,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 1,
                chase = true,
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
                aspd = 0,
                chase = true,
                datk = false,
                crit = 10,
                shield = 0
            }
            ),
            0.9f - 0.001f,
            0.9f + 0.001f);
    }

    [Fact]
    public void TestNoishShark()
    {
        ColCalModel model = new ColCalModel();
        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 33,
                hit = 74,
                atc = 23,
                def = 8,
                aspd = -4,
                chase = false,
                datk = false,
                crit = 7,
                shield = 0
            },
            new ColCalModel.Parameter()
            {
                hp = 40,
                hit = 68,
                atc = 20,
                def = 8,
                aspd = 10,
                chase = true,
                datk = false,
                crit = 0,
                shield = 0
            }
            ),
            0.136f,
            0.137f);
    }

    [Fact]
    public void TestEnemyShield()
    {
        ColCalModel model = new ColCalModel();
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
                aspd = 1,
                chase = false,
                datk = false,
                crit = 0,
                shield = 20
            }
            ),
            0.8f - 0.001f,
            0.8f + 0.001f);


        Assert.InRange(model.Calc(
            new ColCalModel.Parameter()
            {
                hp = 2,
                hit = 100,
                atc = 2,
                def = 1,
                aspd = 0,
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
                shield = 20
            }
            ),
            0.8f - 0.001f,
            0.8f + 0.001f);
    }
}
