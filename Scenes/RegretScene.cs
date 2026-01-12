using System;

public class RegretScene : Scene
{
    // 규칙 설정

    //플레이어 hp와 gold
    private int hp;
    private int gold;

    
    private int way; // 0 위, 1 중간, 2 아래
    // 현재 플레이어의 선택
    private int pos; // 현재 위치 (0 ~ 10)

    private bool isInWaySelection; // Ture면 Way 길 선택 구간( 위아래로 이동 가능)

    private bool isWayLocked; // 길이 고정 되어 있는지 확인(앞으로 가면 ture) 시작 점까지 오기 전에 되돌아 올 수 없음
    
    
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
    }

    public override void Update()
    {
        // 매프레임 갱신 되어야 할 것
        if (hp <= 0)
        {
            EndScene.IsDead = true;
            EndScene.LastGold = gold;
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
            //isWayLocked = true; // 플레이어가 가는 길 고정시키기
        }

        // 왼쪽 이동 (후퇴, HP 감소) - Way 선택 구간이 아닐때만 가능
        if (InputManager.GetKey(ConsoleKey.LeftArrow) && !isInWaySelection && pos > 0)
        {
            pos--;
            hp--;
        }

        // 문 열기 (문 앞에서 Enter) - Way 선택 구간이 아닐때만
        if (InputManager.GetKey(ConsoleKey.Enter) && !isInWaySelection && pos == DOOR_POS)
        {
            doorPress++;

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

                // 다음 구간으로 - 다시 Way 선택 구간으로
                isInWaySelection = true;
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
            SceneManager.Change("End");
        }
    }

    public override void Render()
    {
        Console.WriteLine("===== 절대 후회하지마 (플레이) =====");
        Console.WriteLine($"HP: {hp}   GOLD: {gold}");
        Console.WriteLine("조작: ↑↓ 선택 / → 진행 / ← 후퇴 / Enter 문");
        Console.WriteLine();

        //Way 선택 구간 표시
        if (isInWaySelection)
        {
            Console.WriteLine("                                                           ");
            Console.WriteLine(">>> Way 선택 구간 - 길을 선택하고 Enter를 눌러 진행 가능합니다.");
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