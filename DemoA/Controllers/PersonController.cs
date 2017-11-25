using CommonLib;
using Dapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DemoA.Controllers
{
    [Route("api/demoa/[controller]")]
    public class PersonController : Controller
    {
        private ICapPublisher capPublisher;

        public PersonController(ICapPublisher _publisher)
        {
            this.capPublisher = _publisher;
        }

        [HttpPost]
        public JsonResult Post()
        {
            var sqlOptions = (SqlServerOptions)HttpContext.RequestServices.GetService(typeof(SqlServerOptions));
            var cstr = sqlOptions.ConnectionString;
            using (var sqlconnection = new SqlConnection(cstr))
            {
                sqlconnection.Open();
                using (var sqlTrans = sqlconnection.BeginTransaction())
                {
                    try
                    {
                        List<Person> persons = new List<Person>();
                        for (int i = 0; i < 10000; i++)
                        {
                            var person = new Person { Name = $"Name_{i.ToString()}", Age = i };
                            var inserted_person = PersonInsert(sqlTrans, person);
                            capPublisher.Publish("", inserted_person, sqlTrans);
                            persons.Add(inserted_person);
                            Console.WriteLine(inserted_person);
                        }
                        sqlTrans.Commit();
                        return Json(persons);
                    }
                    catch (Exception ex)
                    {
                        sqlTrans.Rollback();
                        return Json(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 本地业务逻辑
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [NonAction]
        private Person PersonInsert(SqlTransaction trans, Person person)
        {
            var sqlconnection = trans.Connection;
            var sql = $"insert into person (Name, Age) output inserted.id, inserted.name, inserted.age values ('{person.Name}', {person.Age})";
            var result = sqlconnection.QueryFirstOrDefault<Person>(sql: sql, transaction: trans);
            return result;
        }

        //没有用到
        [NonAction]
        [CapSubscribe("RemovePerson")]
        public void RemovePerson(Person person)
        {
            var cstr = "Data Source=192.168.0.250;Initial Catalog=CapDemo;User ID=sa;Password=123123;";
            using (var sqlconnection = new SqlConnection(cstr))
            {
                sqlconnection.Open();
                using (var trans = sqlconnection.BeginTransaction())
                {
                    try
                    {
                        int row_count = sqlconnection.Execute($"delete from Person where id={person.id}", null, trans);
                        if (row_count == 1)
                        {
                            Console.WriteLine("删除记录成功, {0}", person.ToString());
                        }
                        else
                            Console.WriteLine("删除记录失败, {0}", person.ToString());
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

        ///// <summary>
        ///// 订阅者业务
        ///// </summary>
        ///// <param name="person"></param>
        //[NonAction]
        //[CapSubscribe("CapDemo")]
        //public Person ReceiveMessage(Person person)
        //{
        //    var cstr = "Data Source=192.168.0.250;Initial Catalog=CapDemo;User ID=sa;Password=123123;";

        //    using (var sqlconnection = new SqlConnection(cstr))
        //    {
        //        sqlconnection.Open();
        //        using (var trans = sqlconnection.BeginTransaction())
        //        {
        //            try
        //            {
        //                var inserted_person = sqlconnection.QueryFirstOrDefault<Person>("insert into Person(Name, Age) OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Age values(@Name, @Age)", person, trans);
        //                Console.WriteLine("received message: {0}", inserted_person.ToString());
        //                trans.Commit();
        //                return inserted_person;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                trans.Rollback();
        //                return null;
        //            }
        //        }
        //    }
        //}


    }
}
