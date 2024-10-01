using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Net.Http;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Diagnostics.Metrics;
using Nest;
using Microsoft.AspNetCore.Http.HttpResults;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;

namespace WebApplicationDaily.dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpGet(Name = "News")]
        public IEnumerable<news> GetNews()
        {
            
                var result_news = new List<news>();
                var connectionstring = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True";
                var connection = new SqlConnection(connectionstring);
                connection.Open();
                var command = new SqlCommand("select * from News", connection);
                var reader = command.ExecuteReader();
               
            
                while (reader.Read())
                {
                    result_news.Add(new news
                    {
                        NewsID = int.Parse (reader["NewsID"]+""),
                        Title = reader["Title"]+"",
                        Description = reader["Description"]+"",
                        PublicationDate = reader.GetDateTime("PublicationDate"),
                        UpdatedDate = reader.GetDateTime("UpdatedDate"),
                        RSS_ID = int.Parse (reader["RSS_ID"]+""),
                        Content = reader["Content"]+"",
                        ImageURL = reader["ImageURL"]+"",
                        ViewID = int.Parse(reader["ViewID"]+""),
                        LikeID = int.Parse(reader["LikeID"] + ""),
                        CommentID = int.Parse(reader["CommentID"] + ""),
                    });
                }
                connection.Close();
            return result_news;
        }

        [HttpGet("{id}/results")]
        public IActionResult GetResults(int id)
        {

            var connectionstring = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True";
            var connection = new SqlConnection(connectionstring);
            connection.Open();
            string CmdText = string.Format(@"select  
                                                    count (NewsID) as countID
                                             from News 
                                             where NewsID = {0}", id);
            var command = new SqlCommand(CmdText, connection);
            var reader = command.ExecuteReader();
            var result = new List<countnewsid>();

            while (reader.Read())
            {
                result.Add(new countnewsid
                {
                    count = reader.GetInt32(0)
                });
            }
            if (!result.Any())
            {
                return NotFound($"No results found {id}");
            }
            return Ok(result);
        }
    }
}




