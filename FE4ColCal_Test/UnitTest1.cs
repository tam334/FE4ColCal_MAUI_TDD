namespace FE4ColCal_Test;
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
    }
}
