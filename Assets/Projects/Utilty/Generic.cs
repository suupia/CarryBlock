using System;

// 参考サイト: https://kou-yeung.hatenablog.com/entry/2013/12/11/230000

namespace Projects.Utilty
{
    public static class Generic
    {
        // Param:params
        public static T Construct<T,P1>(params P1[] p1)
        {
            return (T)typeof(T).GetConstructor(new Type[]{typeof(P1[])}).Invoke(new object[]{p1});
        }
        // Param:0
        public static T Construct<T>() where T : new()
        {
            return new T();
        }
        // Param:1
        public static T Construct<T,P1>(P1 p1)
        {
            return (T)typeof(T).GetConstructor(new Type[]{typeof(P1)}).Invoke(new object[]{p1});
        }
        // Param:2
        public static T Construct<T,P1,P2>(P1 p1,P2 p2)
        {
            return (T)typeof(T).GetConstructor(new Type[]{typeof(P1),typeof(P2)}).Invoke(new object[]{p1,p2});
        }
        // Param:3
        public static T Construct<T,P1,P2,P3>(P1 p1,P2 p2,P3 p3)
        {
            return (T)typeof(T).GetConstructor(new Type[]{typeof(P1),typeof(P2),typeof(P3)}).Invoke(new object[]{p1,p2,p3});
        }
    }
    
    // 使い方
    // // 可変長引数
    // Params param = Generic.Construct<Params,int>(0,1,2,3,4,5,6,7,8,9);
    // // 引数１個
    // Param1 param_int = Generic.Construct<Param1,int>(0);
    // // 引数２個
    // Param2 param2 = Generic.Construct<Param2,int,string>(0,"Hello");
}