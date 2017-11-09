# Frends.Community.Oracle
FRENDS community task for Oracle SQL operations

## Installing
You can install the task via FRENDS UI Task View or you can find the nuget package from the following nuget feed
'Insert nuget feed here'

## Building
Clone a copy of the repo

`git clone https://github.com/CommunityHiQ/Frends.Community.Oracle.QueryData.git`

Restore dependencies

`nuget restore frends.community.oracle.querydata`

Rebuild the project

Run Tests with nunit3. Tests can be found under

`Frends.Community.Oracle.QueryData.Tests\bin\Release\Frends.Community.Oracle.QueryData.Tests`

Create a nuget package

`nuget pack nuspec/Frends.Community.Oracle.QueryData.nuspec`

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### Task Properties

#### Input

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| ConnectionString | string | Connection string to the oracle database | Data Source=localhost;User Id=<userid>;Password=<password>;Persist Security Info=True; |
| Query | string | The SQL query to perform | SELECT * FROM Table WHERE Column = 'Value' |

#### Options

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| RootElementName | string | The name of the root element of the resultset | RowSet |
| RowElementName | string | The name of the row element(s) in the resultset | Row |
| MaximumRows | integer | The maximum amount of rows to return | 666 |
| TimeoutSeconds | integer | The amount of seconds to let a query run before timeout | 666 |
| Parameters | OracleParameter[] |  Array with the oracle parameters | n/a |
| ReturnType | OracleQueryReturnType | Specifies in what format to return the results | XMLDocument |

#### OracleParameter

| Property             | Type                 | Description                          | Example |
| ---------------------| ---------------------| ------------------------------------ | ----- |
| Name | string | Name of the parameter | ParamName |
| Value | dynamic | Value of the parameter | 1 |
| DataType | ParameterDataType | Specifies the Oracle type of the parameter using the ParameterDataType enumeration | NVarchar |

### Oracle.QueryData.PerformQuery

#### Example usage

#### Result

| Property/Method | Type | Description | Example |
| ---------------------| ---------------------| ----------------------- | -------- |
| Success | boolean | Task execution result. | true |
| Message | string | Failed task execution message (if throw exception on error is false). | "Connection failed" |
| Result | variable | The resultset in the format specified in the Options of the input | <?xml version="1.0"?><root> <row>  <ID>0</ID>  <TABLEID>20013</TABLEID>  <FIELDNAME>AdminStatus</FIELDNAME>  <CODE>0</CODE>  <ATTRTYPE>0</ATTRTYPE>  <ACTIVEUSE>1</ACTIVEUSE>  <LANGUAGEID>fin</LANGUAGEID> </row></root>|