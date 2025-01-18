using Dapper;
using Grpc.Core;
using GrpcServiceInteractingBetweenUsers.Models;
using Npgsql;
using System.Text;

namespace GrpcServiceInteractingBetweenUsers.Extensions
{
    public class InsertRecordToDatabase
    {
        public static void InsertToNotificationTable<T>(NpgsqlConnection connection, T insertedNotificationRecord, string tableName) where T : class
        {
            var getInsertedProperties = insertedNotificationRecord.GetType().GetProperties();

            var insertedQuery = new StringBuilder();
            insertedQuery.Append("INSERT INTO \"");
            insertedQuery.Append(tableName);
            insertedQuery.Append("\" ");
            insertedQuery.Append("(");
            for (int i = 0; i < getInsertedProperties.Length; i++)
            {
                insertedQuery.Append("\"");
                insertedQuery.Append(getInsertedProperties[i].Name);
                insertedQuery.Append("\"");
                if (i != getInsertedProperties.Length - 1)
                {
                    insertedQuery.Append(", ");
                }
            }
            insertedQuery.Append(")");

            insertedQuery.Append(" VALUES(");

            for (int i = 0; i < getInsertedProperties.Length; i++)
            {
                insertedQuery.Append("@");
                insertedQuery.Append(getInsertedProperties[i].Name);
                if (i != getInsertedProperties.Length - 1)
                {
                    insertedQuery.Append(", ");
                }
            }
            insertedQuery.Append(")");

            connection.Execute(insertedQuery.ToString(), insertedNotificationRecord);
        }
    }
}
