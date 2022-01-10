namespace Common.Core.Execution;

public delegate Task<TResult> AsyncFunc<TResult>();

public delegate Task<TResult> AsyncFunc<in T1, TResult>(T1 arg1);

public delegate Task<TResult> AsyncFunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);