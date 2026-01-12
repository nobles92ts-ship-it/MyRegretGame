using System;
using System.Collections.Generic;

// 문자열 등록하고 원할때 전환이 필요함

public static class SceneManager
{
    //딕셔너리에 이름으로 찾도록 만들어야함
    private static Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

    //현재 활성화 된 씬
    private static Scene _current;

    // 씬 이름을 등록하고 이름으로 호출한다 이하기 쉽게
    public static void AddScene(string name, Scene scene)
    {
        _scenes[name] = scene;
    }

    // 씬 전환
    public static void Change(string name)
    {
        if (_current != null)
            _current.Exit();

        _current = _scenes[name];
        _current.Enter();
    }

    //매 프레임 호출
    // 현재 씬의 로직처리
    public static void Update()
    {
        _current.Update();
    }

    // 현재 화면 출력
    // 프레임 별로 호출
    public static void Render()
    {
        Console.Clear();
        _current.Render();
    }
}