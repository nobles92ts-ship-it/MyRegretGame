using System;

// 게임 시작 화면 
// 튜토리얼 설명 
// 시작도 해야함


public class TitleScene : Scene
{
    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.Enter))
        {
            SceneManager.Change("Regret");
        }
    }

    // 튜토리얼 , 시작

    public override void Render()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("             후회하지마            ");
        Console.WriteLine("==================================");
        Console.WriteLine();
        Console.WriteLine("Enter : 게임 시작");
        Console.WriteLine();
        Console.WriteLine("규칙 요약:");
        Console.WriteLine("- 오른쪽/위/아래 이동 가능");
        Console.WriteLine("- 10칸 이동 후 문 앞에서 Enter 누르면 문을 열 수 있습니다.");
        Console.WriteLine("- 길에 따라 골드 보상 변화");
    }
}