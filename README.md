## What is QueryMaker?

QueryMaker is a .NET library built from the ground up using LINQ Expressions which provides the tools for making complex queries in a dynamic fashion via simple components. These components allow for performing multi-layered filtering with automatic type conversions, sorting, selecting and paging.

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

Now that you have the `QueryMakerLibrary` on your project, you can make a query using the `MakeQuery()` method as shown on the following sample:

  ```csharp
  // first, we create an instance of QueryMaker with the components we want to use
  // they are all optional, but for this sample we will use them all
  QueryMaker query = new QueryMaker (
    // let's say we want to filter the users which their DateOfBirth is Greater Than Or Equal to May 1990
    // and also their FirstName OR LastName contains 'John' or 'Doe'
    filter: new Filter(
      field: "DateOfBirth",
      action: FilterActions.GreaterThanOrEqual,
      value: "1990-05",
      and: new Filter(
        fields: new string[] { "FirstName", "LastName" },
        action: FilterActions.Contains,
        value: new object?[] { "JOHN", "DoE" },
        ignoreCase: true)),

    // we want them sorted by FirstName on ascending direction
    // and then sorted by DateOfDeath on a descending direction
    sort: new Sort(
      field: "FirstName",
      then: new Sort(
        field: "DateOfDeath",
        direction: SortDirections.Descending)),

    // also, we want to page the results by skipping 2 and taking 5
    page: new Page(skip: 2, take: 5),

    // finally, we want to explicitly select the following fields
    select: new Select("Id", "FirstName", "Email", "Phone"));

  // second, we pass this query we created to the MakeQuery() method on an IQueryable instance
  return await _dataContext.Users.MakeQuery(query).ToListAsync();
  ```

We can also create the same QueryMaker instance as a JSON (see [sample.json](https://github.com/PRLL/QueryMaker/blob/main/sample.json)):

  ```json
  {
    "filter": {
      "field": "DateOfBirth",
      "action": 7,
      "value": "1990-05",
      "and": {
        "fields": [ "FirstName", "LastName" ],
        "action": 1,
        "value": [ "JOHN", "DoE" ],
        "ignoreCase": true
      }
    },
    "sort": {
      "field": "FirstName",
      "then": {
        "field": "DateOfDeath",
        "direction": 2
      }
    },
    "page": {
      "skip": 2,
      "take": 5
    },
    "select": {
      "fields": [ "Id", "FirstName", "Email", "Phone" ]
    }
  }
  ```

And then receive this JSON on an API endpoint and make the query dynamically:

  ```csharp
  [HttpPost(Name = "filterUsers")]
  public async Task<IActionResult> FilterUsers([FromBody] QueryMaker query)
  {
    return Ok(await _dataContext.Users.MakeQuery(query).ToListAsync());
  }
  ```

In both cases, Entity Framework will generate the following SQL query:

  ```sql
  SELECT [u].[Id], [u].[FirstName], [u].[Email], [u].[Phone]
  FROM [Users] AS [u]
  WHERE ([u].[DateOfBirth] >= '1990-05-01T00:00:00.0000000') AND
    (((LOWER([u].[FirstName]) LIKE N'%john%') OR (LOWER([u].[FirstName]) LIKE N'%doe%'))
    OR ((LOWER([u].[LastName]) LIKE N'%john%') OR (LOWER([u].[LastName]) LIKE N'%doe%')))
  ORDER BY [u].[FirstName], [u].[DateOfDeath] DESC
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
- [x] Add functionality for performing pagination using an index
- [x] Create method implementations for adding components on a QueryMaker instance
- [x] Add support for IEnumerable and it's inherited interfaces (IList, ICollection...)
- [ ] Create documentation/tutorial for using the library
- [ ] Add static configuration class for setting defaults (i.e. IgnoreCase = true/false by default...)

<br />



## License

Distributed under the GNU General Public License v3.0 License. See `LICENSE.txt` for more information.

<br />



## Contact

LinkedIn: [Jose Toyos](https://www.linkedin.com/in/josetoyosvargas/)

Email: josemoises.toyosvargas@hotmail.com

Project Link: [https://github.com/PRLL/QueryMaker](https://github.com/PRLL/QueryMaker)

<br />



## Copyright

©Jose Toyos 2026
