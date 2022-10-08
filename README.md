## What is QueryMaker?

QueryMaker is a .NET library built from the ground up using LINQ Expressions which provides the tools for making complex queries in a dynamic fashion via simple components. This components allow for performing multi-layered filtering with automatic type conversions, sorting, selecting and paging.

<br />



## Installation

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [QueryMaker](https://www.nuget.org/packages/QueryMaker/) from the package manager console:

  ```
  PM> Install-Package QueryMaker
  ```

  Or from the .NET CLI as:

  ```powershell
  dotnet add package QueryMaker
  ```

<br />



## How To Use

Now that you have the `QueryMakerLibrary` on your project, you can make a query using the `MakeQuery()` method.  as shown on the following sample:

  ```csharp
  // first, we create an instance of QueryMaker with the components we want to use
  // they are all optional, but for this sample we will use them all
  QueryMaker query = new QueryMaker (
    // say we want to filter the users which their DateOfBirth is Greater Than Or Equal to May 1990
    // AND their FirstName OR LastName contains 'John' or 'Doe'
    new Filter
    {
      Fields = new string[] { "DateOfBirth" },
      Value = "1990-05",
      Action = FilterActions.GreaterThanOrEqual,
      SubFiltersOperation = FilterOperations.AndAlso,
      SubFilters = new Filter[]
      {
        new Filter
        {
          Fields = new string[] { "FirstName", "LastName" },
          FieldsOperation = FilterOperations.OrElse,
          Value = new object?[] { "JOHN", "DoE" },
          Action = FilterActions.Contains,
          IgnoreCase = true
        }
      }
    },

    // we also want to sort them by Id on a descending direction
    new Sort("Id", SortDirections.Descending),

    // then, we want to select the following fields
    new Select(new string[] { "Id", "FirstName", "Email", "Phone" }),

    // and finally, we want to page the results by skipping 2 and taking 5
    new Page(2, 5));

  // we can then pass this query we created to the MakeQuery() method on an IQueryable instance
  return await _dataContext.Users.MakeQuery(query).ToListAsync();
  ```

We can also create the same QueryMaker instance as a JSON (see [sample.json](https://github.com/PRLL/QueryMaker/blob/main/sample.json)):

  ```json
  {
    "filter": {
      "fields": [ "DateOfBirth" ],
      "value": "1990-05",
      "action": 7,
      "subFiltersOperation": 2,
      "subFilters": [
        {
          "fields": [ "FirstName", "LastName" ],
          "fieldsOperation": 1,
          "value": [ "JOHN", "DoE" ],
          "action": 1,
          "ignoreCase": true
        }
      ]
    },
    "sort": {
      "field": "Id",
      "direction": 2
    },
    "select": {
      "fields": [ "Id", "FirstName", "Email", "Phone" ]
    },
    "page": {
      "skip": 2,
      "take": 5
    }
  }
  ```

And then receive this JSON on an API endpoint and make the query dynamically:

  ```csharp
  [HttpPost("filterUsers")]
  public async Task<IActionResult> FilterUsers([FromBody] QueryMaker query)
  {
    return await _dataContext.Users.MakeQuery(query).ToListAsync();
  }
  ```

In both cases, Entity Framework will generate the following SQL query:

  ```sql
  SELECT [u].[Id], [u].[FirstName], [u].[Email], [u].[Phone]
  FROM [Users] AS [u]
  WHERE ([u].[DateOfBirth] >= '1990-05-01T00:00:00.0000000')
    AND ((LOWER([u].[LastName]) LIKE N'%john%') OR (LOWER([u].[LastName]) LIKE N'%doe%'))
  ORDER BY [u].[Id] DESC
  OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
  ```

<br />



## Roadmap

- [x] Add multi-layered filtering
- [x] Add sorting, selecting and paging
- [x] Add automatic type conversions for value(s) and fields
- [x] Add use of arrays for fields and values to perform multiple operations on a single filter
- [x] Add extension to IQueryable interface for direct use on instances
- [x] Add a method which returns the resulting query and the total count of items without pagination
- [ ] Create documentation/tutorial for using the library
- [ ] Add support for IEnumerable and it's inherited interfaces (IList, ICollection...)
- [ ] Add static configuration class for setting defaults (i.e. IgnoreCase = true/false by default...)
- [ ] Add functionality for performing pagination using an index

<br />



## License

Distributed under the GNU General Public License v3.0 License. See `LICENSE.txt` for more information.

<br />



## Contact

LinkedIn: [Jose Toyos](https://www.linkedin.com/in/jose-moises-toyos-vargas-868119182/)

Email: josemoises.toyosvargas@hotmail.com

Project Link: [https://github.com/PRLL/QueryMaker](https://github.com/PRLL/QueryMaker)
