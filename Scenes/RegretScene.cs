using System;

public class RegretScene : Scene
{
    // 규칙 설정

    //플레이어 hp와 gold
    private int hp;
    private int gold;

    // 통계 변수
    private int totalMoves; // 총 이동 횟수
    private int totalDoorPresses; // 총 문 누른 횟수
    private int goldCollected; // 골드 획득 횟수


    private int way; // 0 위, 1 중간, 2 아래
    // 현재 플레이어의 선택
    private int pos; // 현재 위치 (0 ~ 10)

    private bool isInWaySelection; // Ture면 Way 길 선택 구간( 위아래로 이동 가능)

    private bool isWayLocked; // 길이 고정 되어 있는지 확인(앞으로 가면 ture) 시작 점까지 오기 전에 되돌아 올 수 없음
    
    private bool showWayPopup; // Way 선택 구간 팝업 표시 여부
    
    private int position; // 0 ~ 10회?
    // 가는길 10회?

    
    private int doorPress;
    //문을 몇 번 눌렀는지

    
    private bool[] blockedWay;
    //각 길이 막혀 있는지 여부

    
    private int[] doorNeed;
    //각 길의 문이 몇번 눌러야 열리는지

    // 고정 규칙
    private const int WAY_COUNT = 3; //위,중간,아래
    private const int DOOR_POS = 10; // 문까지 가는길 10칸

    
    private Random rand = new Random();

    // 씬 시작 (씬 시작하면 초기화 상태가 되어야함)
    public override void Enter()
    {
        //플레이어 초기화
        hp = 20;
        gold = 0;

        // 통계 초기화
        totalMoves = 0;
        totalDoorPresses = 0;
        goldCollected = 0;

        // 중간에서 시작 가장 왼쪽에서 시작
        way = 1;
        position = 0;
        doorPress = 0;

        //길, 문, 정보
        blockedWay = new bool[WAY_COUNT];
        doorNeed = new int[WAY_COUNT];

        //첫 구간 Way 선택 구간
        isInWaySelection = true;
        pos = 0; 
        showWayPopup = true; // 첫 시작 시 팝업 표시
    }

    public override void Update()
    {
        // Way 선택 구간 팝업 표시 중이면 아무키나 눌러서 닫기
        if (showWayPopup)
        {
            if (InputManager.GetKey(ConsoleKey.UpArrow) ||
                InputManager.GetKey(ConsoleKey.DownArrow) ||
                InputManager.GetKey(ConsoleKey.LeftArrow) ||
                InputManager.GetKey(ConsoleKey.RightArrow) ||
                InputManager.GetKey(ConsoleKey.Enter))
            {
                showWayPopup = false; // 팝업 닫기
            }
            else
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); //키 입력 소비
                showWayPopup = false; // 팝업 닫기
            }
            return; // 팝업 표시 중에는 다른 입력 무시
        }
        // 매프레임 갱신 되어야 할 것
        if (hp <= 0)
        {
            EndScene.IsDead = true;
            EndScene.LastGold = gold;
            EndScene.TotalMoves = totalMoves;
            EndScene.TotalDoorPresses = totalDoorPresses;
            EndScene.GoldCollected = goldCollected;
            SceneManager.Change("End");
            return;
        }

        // 골드를 5번 획득하면 게임 종료
        if (goldCollected >= 5)
        {
            EndScene.IsDead = false;
            EndScene.LastGold = gold;
            EndScene.TotalMoves = totalMoves;
            EndScene.TotalDoorPresses = totalDoorPresses;
            EndScene.GoldCollected = goldCollected;
            SceneManager.Change("End");
            return;
        }

        //길 선택(위 아래)
        if (InputManager.GetKey(ConsoleKey.UpArrow) && way > 0 && (isInWaySelection || pos == 0) )
        {
            way--;
        }

        if (InputManager.GetKey(ConsoleKey.DownArrow) && way < WAY_COUNT - 1 && (isInWaySelection || pos == 0))
        {
            way++;
        }

        // Way 구간에서 엔터키로 길 확정
        if (InputManager.GetKey(ConsoleKey.Enter) && isInWaySelection)
        {
            isInWaySelection = false; // Way 선택 완료 되면 진행 시작
            MakeStage(); // 스테이지 생성
        }
        
        // 오른쪽 이동 (앞으로) - Way 구간이 아닐때만 가능
        if (InputManager.GetKey(ConsoleKey.RightArrow) && pos < DOOR_POS && !isInWaySelection)
        {
            pos++;
            totalMoves++; // 이동 횟수 카운트
            //isWayLocked = true; // 플레이어가 가는 길 고정시키기
        }

        // 왼쪽 이동 (후퇴, HP 감소) - Way 선택 구간이 아닐때만 가능
        if (InputManager.GetKey(ConsoleKey.LeftArrow) && !isInWaySelection && pos > 0)
        {
            pos--;
            hp--;
            totalMoves++; // 이동 횟수 카운트
        }

        // 문 열기 (문 앞에서 Enter) - Way 선택 구간이 아닐때만
        if (InputManager.GetKey(ConsoleKey.Enter) && !isInWaySelection && pos == DOOR_POS)
        {
            doorPress++;
            totalDoorPresses++; // 총 문 두드린 횟수 카운트

            // 문이 열렸는지 확인
            if (doorPress >= doorNeed[way])
            {
                // 보상 지급
                if (way == 0)
                    gold += 4;  // 위
                else if (way == 1)
                    gold += 1;  // 중간
                else
                    gold -= 1;  // 아래

                goldCollected++; // 골드 획득 횟수 증가

                // 다음 구간으로 - 다시 Way 선택 구간으로
                isInWaySelection = true;
                showWayPopup = true; // Way 선택 구간 팝업 표시
                pos = 0;
                doorPress = 0;
                MakeStage(); //스테이지 생성
                // MakeStage();  Way 선택 후 호출
            }
        }

        if (InputManager.GetKey(ConsoleKey.Escape))
        {
            EndScene.IsDead = false;
            EndScene.LastGold = gold;
            EndScene.TotalMoves = totalMoves;
            EndScene.TotalDoorPresses = totalDoorPresses;
            EndScene.GoldCollected = goldCollected;
            SceneManager.Change("End");
        }
    }

    public override void Render()
    {
        // Way 선택 팝업 표시
        if (showWayPopup)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("        ╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("        ║                                                       ║");
            Console.WriteLine("        ║            >>> Way 선택 구간 <<<                      ║");
            Console.WriteLine("        ║                                                       ║");
            Console.WriteLine("        ║     ↑↓ 방향키로 길을 선택하고                        ║");
            Console.WriteLine("        ║     Enter를 눌러 진행하세요                           ║");
            Console.WriteLine("        ║                                                       ║");
            Console.WriteLine("        ║                                                       ║");
            Console.WriteLine("        ║           아무 키나 눌러 계속...                      ║");
            Console.WriteLine("        ║                                                       ║");
            Console.WriteLine("        ╚═══════════════════════════════════════════════════════╝");
            return; // 팝업 표시 중에는 게임 화면 출력 안함
        }
        // 일반 게임 화면 
        Console.WriteLine("===== VOID (플레이) =====");
        Console.WriteLine($"HP: {hp}   GOLD: {gold}   도전 결과: {goldCollected}/5");
        Console.WriteLine($"이동 횟수: {totalMoves}   문 두드린 횟수: {totalDoorPresses}");
        Console.WriteLine("조작: ↑↓ 선택 / → 진행 / ← 후퇴 / Enter 문");
        Console.WriteLine();

        //Way 선택 구간 표시
        if (isInWaySelection)
        {
            Console.WriteLine("                                                           ");
            Console.WriteLine(">>> Way 선택 구간 - 길을 선택하고 Enter를 눌러 진행 가능합니다.<<<");
        }
        else
        {
            Console.WriteLine("                                                        ");
            Console.WriteLine("                                                        ");
        }

        DrawWay(0, "[오르막길]");
        DrawWay(1, "[평지]");
        DrawWay(2, "[내려막길]");

        Console.WriteLine("                                                        ");
        Console.WriteLine("(현재 단계: 화면 출력 / ESE : 종료)");
    }

    private void DrawWay(int index, string label)
    {
        Console.WriteLine(label + " ");

        for (int x = 0; x <= DOOR_POS; x++)
        {
            if (index == way && x == pos)
                Console.Write('p');

            else if (x == DOOR_POS)
                Console.Write('D');
            else
                Console.Write('_');
        }
        Console.WriteLine("                  "); //라인 끝 공백 구분하기 위해
    }

    private void MakeStage()
    {
        // 새로운 구간 생성
        // 각 길의 문을 오르막길 10번 나머지 1번

        doorNeed[0] = 10; // 오르막 길 10번
        doorNeed[1] = rand.Next(1,11); // 중간 길 랜덤
        doorNeed[2] = rand.Next(1,11); // 아래 길 랜덤



        for (int i = 0; i < WAY_COUNT; i++)
        {
            
            blockedWay[i] = false;
        }

        // 플레이어 위치 초기화
        pos = 0;
        doorPress = 0;
        isWayLocked = false; // 새 스테이지 가서 길 선택 가능하도록
    }
}