namespace FE4ColCal_Test;
using FE4ColCal_MAUI_TDD;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        ColCalModel model = new ColCalModel();

        //一撃でプレイヤーが勝利する
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
}
