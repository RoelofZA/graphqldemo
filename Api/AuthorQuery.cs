using System.Linq;
using Api.Database;
using GraphQL.Types;
using GraphQL.Resolvers;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL
{
  public class AuthorQuery : ObjectGraphType
  {
    public AuthorQuery(ApplicationDbContext db)
    {
      Field<AuthorType>(
        "Author",
        arguments: new QueryArguments(
          new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Author." }),
        resolve: context =>
        {
          var id = int.Parse(context.Arguments["id"].ToString());
          var author = db
            .Authors
            .Include(a => a.Books)
            .FirstOrDefault(i => i.Id == id);
          return author;
        });

      Field<ListGraphType<AuthorType>>(
        "Authors",
        resolve: context =>
        {
          var authors = db.Authors.Include(a => a.Books);
          return authors;
        });

      Field<ListGraphType<BookType>>(
        "Books",
        resolve: context =>
        {
          var books = db.Books;
          return books;
        });
      
    }
  }
}