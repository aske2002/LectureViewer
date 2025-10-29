using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
namespace backend.Domain.Extensions;
public static class IQueryableExtensions
{
    public static List<string> GetIncludes<TEntity>(this IQueryable<TEntity> query)
    {
        var includes = new List<string>();

        if (query is IIncludableQueryable<TEntity, object>)
        {
            var expression = query.Expression;
            ExtractIncludes(expression, includes);
        }

        return includes;
    }

    private static void ExtractIncludes(Expression expression, List<string> includes)
    {
        if (expression is MethodCallExpression methodCall)
        {
            // Check for Include or ThenInclude
            if (methodCall.Method.Name == "Include" || methodCall.Method.Name == "ThenInclude")
            {
                if (methodCall.Arguments[1] is UnaryExpression unary && unary.Operand is LambdaExpression lambda)
                {
                    var path = GetPropertyPath(lambda.Body);
                    if (!string.IsNullOrEmpty(path))
                    {
                        includes.Add(path);
                    }
                }
            }

            ExtractIncludes(methodCall.Arguments[0], includes); // Keep walking down the tree
        }
    }

    private static string GetPropertyPath(Expression? expression)
    {
        var parts = new List<string>();
        while (expression is MemberExpression member)
        {
            parts.Insert(0, member.Member.Name);
            expression = member.Expression;
        }
        return string.Join(".", parts);
    }
}
