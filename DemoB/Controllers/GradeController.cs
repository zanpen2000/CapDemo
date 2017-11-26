using System;
using Microsoft.AspNetCore.Mvc;
using CommonLib;
using DotNetCore.CAP;
using System.Data.SqlClient;
using Dapper;

namespace DemoB.Controers
{
    [Route("api/demob/[controller]")]
    public class GradeController : Controller
    {
        [NonAction]
        [CapSubscribe("CapPerson")]
        public void ReceiveMessage(Person person)
        {
            Console.WriteLine("this.HttpContext =>  {0}", this.HttpContext);

            Console.WriteLine("Grade service 接收:" + person);
            var cstr = "Data Source=192.168.0.250;Initial Catalog=cap_trans_test;User ID=sa;Password=123123;";

            using (var sqlconnection = new SqlConnection(cstr))
            {
                sqlconnection.Open();
                using (var trans = sqlconnection.BeginTransaction())
                {
                    try
                    {
                        var grade = new Grade { personId = person.id, name = "新增班级" };
                        var inserted = sqlconnection.QueryFirstOrDefault<Grade>("insert into Grade(personId,Name) output inserted.id, inserted.personId,inserted.Name values(@personId,@Name)", grade, trans);
                        Console.WriteLine("Grade Inserted: {0}", grade);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }
                }
            }
        }
    }
}
