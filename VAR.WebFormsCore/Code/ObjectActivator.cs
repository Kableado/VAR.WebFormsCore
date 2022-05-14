using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VAR.WebFormsCore.Code
{
    public static class ObjectActivator
    {
        private static readonly Dictionary<Type, Func<object>> Creators = new Dictionary<Type, Func<object>>();

        private static Func<object> GetLambdaNew(Type type)
        {
            lock (Creators)
            {
                if (Creators.ContainsKey(type)) { return Creators[type]; }

                NewExpression newExp = Expression.New(type);
                LambdaExpression lambda = Expression.Lambda(typeof(Func<object>), newExp);
                Func<object> compiledLambdaNew = (Func<object>) lambda.Compile();

                Creators.Add(type, compiledLambdaNew);

                return Creators[type];
            }
        }

        public static object CreateInstance(Type type)
        {
            Func<object> creator = GetLambdaNew(type);
            return creator();
        }
    }
}