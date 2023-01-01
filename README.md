# Kogane Ping Utils

UnityEngine.Ping を非同期処理で呼び出せるようにするクラス

## 使用例

```cs
using Cysharp.Threading.Tasks;
using Kogane;

public static class InternetChecker
{
    private static readonly string[] ADDRESSES =
    {
        "8.8.8.8", // Google Public DNS
        "8.8.4.4", // Google Public DNS
        "4.2.2.2", // Level 3 Communications
        "4.2.2.3", // Level 3 Communications
        "4.2.2.4", // Level 3 Communications
    };

    public static async UniTask<bool> IsOnlineAsync( float timeoutSeconds )
    {
        foreach ( var address in ADDRESSES )
        {
            var result = await PingUtils.SendAsync( address, timeoutSeconds );

            if ( result.IsSuccess ) return true;
        }

        return false;
    }
}
```

```cs
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Example : MonoBehaviour
{
    private async UniTaskVoid Start()
    {
        Debug.Log( await InternetChecker.IsOnlineAsync( 5 ) );
    }
}
```

## 依存しているパッケージ

* https://github.com/Cysharp/UniTask