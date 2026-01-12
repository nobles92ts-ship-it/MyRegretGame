using System;

// 조건에 맞으면 루프를 계속 돌아야함 
// while, for 뭐가 맞는지 고민이 필요함
// 입역을 해준것만 갱신해야함 없으면 두고
// 유저가 입력한 거에 맞게 움직여야함, 한개만 입력 되고 저장함


public static class InputManager
{
    private static ConsoleKey? _lastKey;
    // 입력 있으면 저장 없으면 null
    public static void ReadInput()
    {
        _lastKey = null;

        if (Console.KeyAvailable)
        {
            //ture면 화면에 글자 안찍히게
            _lastKey = Console.ReadKey(true).Key;
        }
    }

    //이번에 눌리게 맞는지 확인
    public static bool GetKey(ConsoleKey key)
    {
        return _lastKey.HasValue && _lastKey.Value == key;
    }
    public static bool HasInput()
    {
        return _lastKey.HasValue;
    }
}