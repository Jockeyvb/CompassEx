
FreeSql.Generator  -Match "^(tbl_|View_).+"  -Filter StoreProcedure -Razor "__razor.cshtml.txt" -NameSpace CompassEx.Data.Models -DB "Sqlite,Data Source=F:\Project\Program\CompassEx\CompassEx.Data\JockeyCalendar.db3;Cache=Shared;Mode=ReadWriteCreate;" -FileName "{name}.cs"
