using System;
using System.Threading;
// 전체적인 메인 루프 담당
// 씬등록/시작 씬 설정 여기서 해서 구조는 변하지 않게

// 입력 읽기
// 씬 업데이트
// 씬 랜더 반복

public class GameManager
{
    //시작 씬과 씬 등록
    public void Init()
    {
        SceneManager.AddScene("Title", new TitleScene());
        SceneManager.AddScene("Regret", new RegretScene());
        SceneManager.AddScene("End", new EndScene());
        SceneManager.Change("Title");
    }
    //반복문 하고 ture면 게임이 진행되어야함
    public void Run()
    {
        Init();
        SceneManager.Render(); // 초기 타이틀 화면 표시

        while(true)
        {
            InputManager.ReadInput(); // 유저 입력 읽기

            if (InputManager.HasInput()) //유저가 입력한다면
            {
                SceneManager.Update(); // 구성에 맞게 갱신
                SceneManager.Render(); // 화면에 출력
            }

            Thread.Sleep(100); // 100ms 딜레이 (초당 10프레임)
        }
    }
}