using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kogane
{
    /// <summary>
    /// UnityEngine.Ping を非同期処理で呼び出せるようにするクラス
    /// </summary>
    public static class PingUtils
    {
        //================================================================================
        // 構造体
        //================================================================================
        [Serializable]
        public struct Result
        {
            [SerializeField] private bool m_isSuccess;
            [SerializeField] private int  m_timeMilliseconds;

            /// <summary>
            /// ping が届いて返ってきたら true を返します
            /// </summary>
            public bool IsSuccess => m_isSuccess;

            /// <summary>
            /// ping が返ってくるまでにかかった時間（ミリ秒）を返します
            /// </summary>
            public int TimeMilliseconds => m_timeMilliseconds;

            internal Result
            (
                bool isSuccess,
                int  timeMilliseconds
            )
            {
                m_isSuccess        = isSuccess;
                m_timeMilliseconds = timeMilliseconds;
            }

            public override string ToString()
            {
                return JsonUtility.ToJson( this, true );
            }
        }

        //================================================================================
        // 関数(static)
        //================================================================================
        /// <summary>
        /// IP アドレスに対して ping を実行します
        /// </summary>
        public static async UniTask<Result> SendAsync( string address, float timeoutSeconds )
        {
            // 機内モードや Wi-Fi がオフならインターネットに接続していません
            if ( Application.internetReachability == NetworkReachability.NotReachable ) return new( false, -1 );

            // ping を飛ばすことでインターネットに接続しているかどうか確認します
            var ping      = new Ping( address );
            var startTime = Time.time;

            while ( true )
            {
                // ping が届いて返ってきたらインターネットに接続しているとみなします
                if ( ping.isDone ) return new( true, ping.time );

                // タイムアウトするまで待機します
                if ( Time.time - startTime < timeoutSeconds )
                {
                    await UniTask.Yield();
                }
                // タイムアウトしたらインターネットに接続していないとみなします
                else
                {
                    return new( false, -1 );
                }
            }
        }
    }
}