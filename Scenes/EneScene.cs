using System;

//엔딩 화면에서 보여주는 화면
//RegretScene에서 결과값을 static으로 전달받아 출력한다.
//Enter: Title로 돌아가기

public class EndScene : Scene
{
    //게임 종료 - 넣어야 함
    public static bool IsDead;
    public static int LastGold;

    // Enter로 다시 타이틀로 돌아오기
    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.Enter))
        {
            SceneManager.Change("Title");
        }
    }
    // 사망 결과와  최종 보상 등 결과 보여주기
    public override void Render()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("               결과               ");
        Console.WriteLine("==================================");
        Console.WriteLine(IsDead ? "체력이 0이 되어 사망했다." : "게임이 종료되었다.");
        Console.WriteLine("남들이 보기에 뒤돌아가는 선택 처럼 보일 수 있다.\n 하지만 잠시 멈추고 돌아보는 시간은 헛된 후퇴가 아니라 더 멀리 나아가기 위한 준비다\n지금 배우는 것이 당장 돈이 되지 않더라도, 묵묵히 문을 두드린 노력은 쉬운 길보다\n더 좋은 결과로 돌아온다 \n그래서 내인 생은 멈추지 않는다.");
        Console.WriteLine($"최종 골드: {LastGold}");
        Console.WriteLine();
        Console.WriteLine("Enter : 타이틀로");
    }
}