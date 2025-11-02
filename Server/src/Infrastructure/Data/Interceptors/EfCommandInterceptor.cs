using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class EfCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Console.WriteLine($"SQL executing: {command.CommandText}");
        return base.ReaderExecuting(command, eventData, result);
    }

    public override DbDataReader ReaderExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result)
    {
        Console.WriteLine($"SQL executed in {eventData.Duration.TotalMilliseconds} ms");
        return result;
    }
}
