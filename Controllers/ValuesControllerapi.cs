using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mycollectionproject.Models;
using System.Collections.Generic;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Mycollectionproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesControllerapi : ControllerBase
    {
        //public readonly string_constr = "Data Source=SL-LAPTOP-D158\\SQLEXPRESS;Initial Catalog=Shrutika;Integrated Security=True;TrustServerCertificate=True;"
        public readonly string _constr;

        public ValuesControllerapi(IConfiguration configuration)
        {
            _constr = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpGet("Items")]
        public IActionResult GetItems()
        {
            List<Collectionform> list = new();

            using var conn = new SqlConnection(_constr);
            SqlCommand cmd = new SqlCommand("select * from CollectionItems C inner join Tags T on T.Tag_id=C.Tag where isactive=1", conn);
            conn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Collectionform
                {
                    id = dr.GetInt32(0),
                    Title = dr.GetString(1),
                    Description = dr.GetString(2),
                    Tag= dr.GetString(9),
                    imageUrl = dr.GetString(4),
                    Link = dr.GetString(5),
                    Created_at = dr.GetDateTime(dr.GetOrdinal("created_at")),
                    Fk_id = dr.GetInt32(8),
                });
            }
            return Ok(list);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using SqlConnection conn = new SqlConnection(_constr);
            string sql = @"SELECT id, Title, Description, Tag, imageUrl, Link, created_at
                   FROM CollectionItems 
                   WHERE id = @id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();

            using SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.Read())
                return NotFound();

            return Ok(new Collectionform
            {
                id = dr.GetInt32(dr.GetOrdinal("id")),
                Title = dr.GetString(dr.GetOrdinal("Title")),
                Description = dr.GetString(dr.GetOrdinal("Description")),
                Tag = dr.GetString(dr.GetOrdinal("Tag")),
                imageUrl = dr.GetString(dr.GetOrdinal("imageUrl")),
                Link = dr.GetString(dr.GetOrdinal("Link")),
                Created_at = dr.GetDateTime(dr.GetOrdinal("created_at"))
            }); 
        }


        [HttpPost("Items")]
        public IActionResult Create([FromBody] Collection c)
        {
            using SqlConnection conn = new SqlConnection(_constr);
            conn.Open();

            if (c.id > 0)
            {
                
                string sqlUpdate = @"UPDATE CollectionItems SET Title = @Title, Description = @Description,Tag = @Tag, imageUrl = @imageUrl,
                                         Link = @Link WHERE Id = @Id";

                using SqlCommand cmd = new SqlCommand(sqlUpdate, conn);
                cmd.Parameters.AddWithValue("@Id", c.id);
                cmd.Parameters.AddWithValue("@Title", c.Title);
                cmd.Parameters.AddWithValue("@Description", c.Description);
                cmd.Parameters.AddWithValue("@Tag", c.Tag);
                cmd.Parameters.AddWithValue("@imageUrl", c.imageUrl);
                cmd.Parameters.AddWithValue("@Link", c.Link);
                cmd.ExecuteNonQuery();

                return Ok("Updated"); 
            }
            else
            {
                
                string sqlInsert = @"INSERT INTO CollectionItems (Title, Description, Tag, imageUrl, Link, isactive)
                                 VALUES (@Title, @Description, @Tag, @imageUrl, @Link, @active)";

                using SqlCommand cmd = new SqlCommand(sqlInsert, conn);
                cmd.Parameters.AddWithValue("@Title", c.Title);
                cmd.Parameters.AddWithValue("@Description", c.Description);
                cmd.Parameters.AddWithValue("@Tag", c.Tag);
                cmd.Parameters.AddWithValue("@imageUrl", c.imageUrl);
                cmd.Parameters.AddWithValue("@Link", c.Link);
                cmd.Parameters.AddWithValue("@active", 1);
                cmd.ExecuteNonQuery();

                return Ok("Created"); 
            }
        }


        [HttpPut("{id}")]
        public IActionResult Update(Collection c)
        {
            using SqlConnection conn = new SqlConnection(_constr);
            string sql = @"UPDATE  CollectionItems
                   SET Title=@Title, Description=@Description, Tag=@Tag, ImageUrl=@ImageUrl, Link=@Link
                   WHERE Id=@Id";  

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", c.Title);
            cmd.Parameters.AddWithValue("@Description", c.Description);
            cmd.Parameters.AddWithValue("@Tag", c.Tag);
            cmd.Parameters.AddWithValue("@ImageUrl", c.imageUrl);        
            cmd.Parameters.AddWithValue("@Link", c.Link);
            cmd.Parameters.AddWithValue("@Id", c.id);                   
            conn.Open();
            cmd.ExecuteNonQuery();

            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using SqlConnection con = new SqlConnection(_constr);
            using SqlCommand cmd = new SqlCommand(
                "update CollectionItems set isactive=0 WHERE id=@id", con);

            cmd.Parameters.AddWithValue("id",id);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("Deleted");
        }
        [HttpGet("Search")]
        public IActionResult Search(string text)

        {
            List<Collection> list = new();


            using SqlConnection con = new SqlConnection(_constr);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Items WHERE Title LIKE '%' + @text + '%' OR Description LIKE '%' + @text + '%'", con);

            cmd.Parameters.Add("@text", SqlDbType.NVarChar).Value = text;

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            var List = new List<Collection>();

            while (dr.Read())
            {
                list.Add(new Collection
                {
                    id = dr.GetInt32(0),
                    Title = dr.GetString(1),
                    Description = dr.GetString(2),
                    Tag = dr.GetString(3),
                    imageUrl = dr.GetString(4),
                    Link = dr.GetString(5)
                });
            }

            return Ok(list);
        }
            [HttpGet("tag/{tag}")]
        public IActionResult GetByTag(int tag_id)
        {
            using SqlConnection conn = new SqlConnection(_constr);
            SqlCommand cmd = new SqlCommand(
                "SELECT id, Title, Description, Tag_id, imageUrl, Link, created_at FROM Items WHERE Tag_id=@tag_id",
                conn
            );

            cmd.Parameters.AddWithValue("@tag_id", tag_id);
            conn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            var items = new List<Collection>();

            while (dr.Read())
            {
                items.Add(new Collection
                {
                    id = dr.GetInt32(dr.GetOrdinal("id")),
                    Title = dr.GetString(dr.GetOrdinal("Title")),
                    Description = dr.GetString(dr.GetOrdinal("Description")),
                    Tag = dr.GetString(dr.GetOrdinal("Tag")),
                    imageUrl = dr.GetString(dr.GetOrdinal("imageUrl")),
                    Link = dr.GetString(dr.GetOrdinal("Link")),
                    Created_at = dr.GetDateTime(dr.GetOrdinal("created_at"))
                });
            }

            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }


    }


}






     

